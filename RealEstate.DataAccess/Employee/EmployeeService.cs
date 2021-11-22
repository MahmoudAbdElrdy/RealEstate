using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data.Models;
using RealEstate.DataAccess.Shared;
using RealEstate.Specifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace RealEstate.DataAccess
{
    [ScopedService]
    public  class EmployeeService
    {
       RealEstateContext _db;
        readonly IMapper _mapper;
        public EmployeeService(RealEstateContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;

        }
        private BaseSpecifications<Employee> Specifications(EmployeeSearch search)
        {
            BaseSpecifications<Employee> specification = null;

            if ((search.DepartmentId!=null&&search.DepartmentId!=0))
            {
                var departmentId = new BaseSpecifications<Employee>(x => x.Department.Id == search.DepartmentId);
                specification = specification?.And(departmentId) ?? departmentId;
            }
            if (!string.IsNullOrEmpty(search.Name))
            {
                var name = new BaseSpecifications<Employee>(x => x.Name.Contains(search.Name));
                specification = specification?.And(name) ?? name;
            }
            if (!string.IsNullOrEmpty(search.Phone))
            {
                var phone = new BaseSpecifications<Employee>(x => x.Phone.Contains(search.Phone));
                specification = specification?.And(phone) ?? phone;
            }
            if ((search.WorkSince)!=null)
            {
                var workSince = new BaseSpecifications<Employee>(x => x.WorkSince.Date.Equals(search.WorkSince.Value.Date));
                specification = specification?.And(workSince) ?? workSince;
            }
            
            if (specification == null)
                specification = new BaseSpecifications<Employee>();
            specification?.AddInclude(x => x.Department);
            specification.isPagingEnabled = true;
            specification.page = search.PageNumber;
            specification.pageSize = search.PageSize;
            return specification;
        }
        public async Task<ResponseData> GetAll(EmployeeSearch search) 
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                IQueryable<Employee> filter;
                
                var specification = Specifications(search);
              
                filter = _db.Employees.Pagtion(specification, out int count);

              //  var entity = _db.Employees.Include(x => x.Department);
                var entity = _mapper.Map<List<EmployeeDto>>(filter);
               
                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = entity,
                    TotalRecordsCount = count,
                    PageCount = (int)Math.Ceiling(count / (double)search?.PageSize),
                    CurrentPage = search?.PageNumber,
                    PageSize = search?.PageSize
                };
            }
            catch (Exception ex)
            {
            
                return new ResponseData
                {
                    IsSuccess = false,
                    Code = EResponse.OK,
                    Message = ex.Message,
                };
            }
        }
        public async Task<ResponseData> GetAllDepartments() 
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start(); 
                var departments = _db.Departments;
              
                var entity = _mapper.Map<List<DropDownListDto>>(departments);
             
                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = entity,
                  
                };
            }
            catch (Exception ex)
            {

                return new ResponseData
                {
                    IsSuccess = false,
                    Code = EResponse.OK,
                    Message = ex.Message,
                };
            }
        }
        public async Task<ResponseData> GetById(int id)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Employee emp =await _db.Employees.Where(a => a.Id == id).FirstOrDefaultAsync();
                var _employee = _mapper.Map<Employee, EmployeeDto>(emp);
                _employee.PassWord = ClsStringEncryptionDecryption.Decrypt(_employee.PassWord, false);
                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = _employee
                };
            }
            catch (Exception ex)
            {

                return new ResponseData
                {
                    IsSuccess = false,
                    Code = EResponse.OK,
                    Message = ex.Message,
                };
            }
          
           
          
           
        }
        public async Task<ResponseData> Delete(int id) 
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Employee emp = await _db.Employees.Where(a => a.Id == id).FirstOrDefaultAsync();
                _db.Employees.Remove(emp);
                _db.SaveChanges();
                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Message="تم الحذف بنجاح"
                };
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;

                if (sqlException != null && sqlException.Number == 547)
                {

                    return new ResponseData
                    {
                        IsSuccess = false,
                        Code = EResponse.UnSuccess,
                        Message = " حدث خطأ لايمكنك الحذف لانه مرتبط ببيانات اخري"
                    };
                }
                else
                {

                    return new ResponseData
                    {
                        IsSuccess = false,
                        Code = EResponse.UnexpectedError,
                        Message = "حدث خطأ لايمكنك الحذف"
                    };
                }
            }
            catch (Exception ex)
            {

                return new ResponseData
                {
                    IsSuccess = false,
                    Code = EResponse.OK,
                    Message = "حدث خطأ  "
                };
            }




        }
        public ResponseData SaveEmployee(EmployeeDto employee)
        {
            if(!string.IsNullOrEmpty(employee.PassWord))
            employee.PassWord = ClsStringEncryptionDecryption.Encrypt(employee.PassWord, false);

            if (employee.Id == 0)
            {
                try
                {
                    if (_db.Employees.Any(x => x.Name.Equals(employee.Name)&& x.Phone.Equals(employee.Phone)))
                    {
                        return new ResponseData { Message = "إسم المستخدم موجود بالفعل", IsSuccess = false };
                    }
                    Employee newRec = new Employee();
                    newRec = _mapper.Map<EmployeeDto, Employee>(employee);
                    newRec.Department = null;
                    _db.Employees.Add(newRec);
                    _db.SaveChanges();
                    return new ResponseData { Message = "تم الحفظ بنجاح", IsSuccess = true };
                }
                catch (Exception ex)
                {
                    throw new NotSupportedException(ex.Message);
                }
            }
            else if (employee.Id != 0)
            {
                Employee newRec = new Employee();

                newRec = _mapper.Map<EmployeeDto, Employee>(employee);

                Employee _newRec = _db.Employees.SingleOrDefault(u => u.Id == employee.Id);
                if (_newRec == null)
                    throw new KeyNotFoundException("غير موجود في قاعدة البيانات");
                //Mapper.Map(ServicesProvider, servicesProvider);
                try
                {
                    _db.Entry(_newRec).CurrentValues.SetValues(newRec);
                    _db.Employees.Attach(_newRec);
                    _db.Entry(_newRec).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _db.SaveChanges();
                    return new ResponseData { Message = "تم الحفظ بنجاح", IsSuccess = true };

                }
                catch (Exception ex)
                {
                    throw new NotSupportedException(ex.Message);
                }
            }
            return new ResponseData { Message = "حدث خطأ", IsSuccess = false };
        }
        public string Getemployee(EmployeeInfo employee)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            employee.PassWord = ClsStringEncryptionDecryption.Encrypt(employee.PassWord, false);
            //BaseSpecifications<Employee> specification = new BaseSpecifications<Employee>(a => a.Name.Equals(employee.Name) && a.PassWord.Equals(employee.PassWord));
            //specification.AddInclude(x => x.Department);
            //Employee emp = _db.Employees.SpecifyFirstOrDefault(specification);
            Employee emp = _db.Employees.Where(a => a.Name.Equals(employee.Name) && a.PassWord.Equals(employee.PassWord)).Include(s=>s.Department).FirstOrDefault();
            var _employee = _mapper.Map<Employee, EmployeeDto>(emp); 
            if (_employee == null) return null;
            _employee.PassWord = ClsStringEncryptionDecryption.Decrypt(employee.PassWord, false);
            var token = clsToken.GenerateToken(_employee.Id.ToString(), emp.Department.Name.ToString(), employee.Name);
                    
            sw.Stop();
            Console.WriteLine("Elapsed={0}", sw.Elapsed);
            return token;
        }
        public async Task<ResponseData> GetDropDownList()
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var dropDownList = _db.Employees;

                var entity = _mapper.Map<List<DropDownListDto>>(dropDownList);

                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = entity,

                };
            }
            catch (Exception ex)
            {

                return new ResponseData
                {
                    IsSuccess = false,
                    Code = EResponse.OK,
                    Message = ex.Message,
                };
            }
        }
    }
}
