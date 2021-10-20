using AutoMapper;
using RealEstate.Data.Models;
using RealEstate.DataAccess.Shared;
using RealEstate.Specifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
        public ResponseData Saveemployee(EmployeeDto employee)
        {
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
                    throw new KeyNotFoundException("employee Not Found In Database");
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
            BaseSpecifications<Employee> specification = new BaseSpecifications<Employee>(a => a.Name.Equals(employee.Name) && a.PassWord.Equals(employee.PassWord));

            Employee emp = _db.Employees.Where(specification.FilterCondition).FirstOrDefault();
            var _employee = _mapper.Map<Employee, EmployeeDto>(emp); 
            if (_employee == null) return null;
            _employee.PassWord = ClsStringEncryptionDecryption.Decrypt(employee.PassWord, false);
            var token = clsToken.GenerateToken(_employee.Id.ToString(), _employee.Department.ToString(), employee.Name);
           
            // employees employee = _db.employees.Where(a => a.employeeName.Equals(employee.employeeName, StringComparison.OrdinalIgnoreCase) && a.Password.Equals(employee.Password, StringComparison.Ordinal)).FirstOrDefault();
            // return _mapper.Map<employees, employeesDTO>(employee);
            
            sw.Stop();
            Console.WriteLine("Elapsed={0}", sw.Elapsed);
            return token;
        }
    }
}
