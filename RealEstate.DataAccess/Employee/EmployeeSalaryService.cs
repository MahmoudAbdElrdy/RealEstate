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
    public  class EmployeeSalaryService
    {
       RealEstateContext _db;
        readonly IMapper _mapper;
        public EmployeeSalaryService(RealEstateContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;

        }
     
        public async Task<ResponseData> GetAll(EmployeeSalarySearch search) 
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                IQueryable<EmployeeSalary> filter;

                BaseSpecifications<EmployeeSalary> specification = new BaseSpecifications<EmployeeSalary>(x=>x.EmployeeId==search.EmployeeId);
               
                if ((search.Date) != null)
                {
                    var dateSalary = new BaseSpecifications<EmployeeSalary>(x => x.Date.Date.Month.Equals(search.Date.Value.Date.Month)&& x.Date.Date.Year.Equals(search.Date.Value.Date.Year));
                    specification = specification?.And(dateSalary) ?? dateSalary;
                }
                specification.isPagingEnabled = true;
                specification.page = search.PageNumber;
                specification.pageSize = search.PageSize;
                filter = _db.EmployeeSalaries.Pagtion(specification, out int count);

              //  var entity = _db.EmployeeSalaries.Include(x => x.Department);
                var entity = _mapper.Map<List<EmployeeSalaryDto>>(filter);
               
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
      
        public async Task<ResponseData> GetById(int id)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                EmployeeSalary emp =await _db.EmployeeSalaries.Where(a => a.Id == id).FirstOrDefaultAsync();
                var _EmployeeSalary = _mapper.Map<EmployeeSalary, EmployeeSalaryDto>(emp);
                        sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = _EmployeeSalary
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
                EmployeeSalary emp = await _db.EmployeeSalaries.Where(a => a.Id == id).FirstOrDefaultAsync();
                _db.EmployeeSalaries.Remove(emp);
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
        public ResponseData SaveEmployeeSalary(EmployeeSalaryDto EmployeeSalary)
        {
         
            if (EmployeeSalary.Id == 0|| EmployeeSalary.Id == null)
            {
                try
                {
                   
                    EmployeeSalary newRec = new EmployeeSalary();
                    newRec = _mapper.Map<EmployeeSalaryDto, EmployeeSalary>(EmployeeSalary);
                    newRec.Employee = null;
                    _db.EmployeeSalaries.Add(newRec);
                    _db.SaveChanges();
                    return new ResponseData { Message = "تم الحفظ بنجاح", IsSuccess = true };
                }
                catch (Exception ex)
                {
                    throw new NotSupportedException(ex.Message);
                }
            }
            else if (EmployeeSalary.Id != 0)
            {
                EmployeeSalary newRec = new EmployeeSalary();

                newRec = _mapper.Map<EmployeeSalaryDto, EmployeeSalary>(EmployeeSalary);

                EmployeeSalary _newRec = _db.EmployeeSalaries.SingleOrDefault(u => u.Id == EmployeeSalary.Id);
                if (_newRec == null)
                    throw new KeyNotFoundException("غير موجود في قاعدة البيانات");
                //Mapper.Map(ServicesProvider, servicesProvider);
                try
                {
                    _db.Entry(_newRec).CurrentValues.SetValues(newRec);
                    _db.EmployeeSalaries.Attach(_newRec);
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
      
    }
}
