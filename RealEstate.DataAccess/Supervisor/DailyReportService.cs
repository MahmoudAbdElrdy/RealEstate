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
    public class DailyReportService
    {
        RealEstateContext _db;
        readonly IMapper _mapper;
        public DailyReportService(RealEstateContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;

        }
        private BaseSpecifications<ViewDailyReport> Specifications(DailyReportSearch search)
        {
            BaseSpecifications<ViewDailyReport> specification = null;

            if (!string.IsNullOrEmpty(search.EmployeeSubmitted))
            {
                var name = new BaseSpecifications<ViewDailyReport>(x => x.EmployeeSubmitted.Contains(search.EmployeeSubmitted));
                specification = specification?.And(name) ?? name;
            }

            if (search.EmployeeId!=null)
            {
                var name = new BaseSpecifications<ViewDailyReport>(x => x.EmployeeId==(search.EmployeeId));
                specification = specification?.And(name) ?? name;
            }
            if (search.SupervisorId != null)
            {
                var name = new BaseSpecifications<ViewDailyReport>(x => x.SupervisorId == (search.SupervisorId));
                specification = specification?.And(name) ?? name;
            }
            if (search.Value != null)
            {
                var name = new BaseSpecifications<ViewDailyReport>(x => x.Value == (search.Value));
                specification = specification?.And(name) ?? name;
            }
            if (search.Date!=null)
            {
                var referrer = new BaseSpecifications<ViewDailyReport>(x => x.Date.Value.Date==search.Date.Value.Date);
                specification = specification?.And(referrer) ?? referrer;
            }


            if (specification == null)
                specification = new BaseSpecifications<ViewDailyReport>();

            specification.isPagingEnabled = true;
            specification.page = search.PageNumber;
            specification.pageSize = search.PageSize;
            return specification;
        }
        public async Task<ResponseData> GetAll(DailyReportSearch search)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                IQueryable<ViewDailyReport> filter;

                var specification = Specifications(search);

                filter = _db.ViewDailyReports.Pagtion(specification, out int count);

             
                var entity = _mapper.Map<List<DailyReportDto>>(filter);

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
                DailyReport emp = await _db.DailyReports.Where(a => a.Id == id).FirstOrDefaultAsync();
                var _DailyReport = _mapper.Map<DailyReport, DailyReportDto>(emp);

                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = _DailyReport
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
                DailyReport emp = await _db.DailyReports.Where(a => a.Id == id).FirstOrDefaultAsync();
                _db.DailyReports.Remove(emp);
                _db.SaveChanges();
                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Message = "تم الحذف بنجاح"
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
        public ResponseData SaveDailyReport(DailyReportDto DailyReport)
        {
            if (DailyReport.Id == 0 || DailyReport.Id == null)
            {
                try
                {

                    DailyReport newRec = new DailyReport();
                    newRec = _mapper.Map<DailyReportDto, DailyReport>(DailyReport);
                    newRec.Employee = null;
                    newRec.Supervisor = null;
                    _db.DailyReports.Add(newRec);
                    _db.SaveChanges();
                    return new ResponseData { Message = "تم الحفظ بنجاح", IsSuccess = true };
                }
                catch (Exception ex)
                {
                    throw new NotSupportedException(ex.Message);
                }
            }
            else if (DailyReport.Id != 0)
            {
                DailyReport newRec = new DailyReport();

                newRec = _mapper.Map<DailyReportDto, DailyReport>(DailyReport);

                DailyReport _newRec = _db.DailyReports.SingleOrDefault(u => u.Id == DailyReport.Id);
                if (_newRec == null)
                    throw new KeyNotFoundException("غير موجود في قاعدة البيانات");
                //Mapper.Map(ServicesProvider, servicesProvider);
                try
                {
                    _db.Entry(_newRec).CurrentValues.SetValues(newRec);

                    _db.DailyReports.Attach(_newRec);
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
        public async Task<ResponseData> GetDropDownList()
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var dropDownList = _db.Projects;

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
