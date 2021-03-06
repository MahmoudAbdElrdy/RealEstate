using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace RealEstate.DataAccess
{
    [ScopedService]
    public class ContractDetailService
    {
        RealEstateContext _db;
        readonly IMapper _mapper;
        public ContractDetailService(RealEstateContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;

        }
        public async Task<ResponseData> GetAllContractDetail(int ContractId)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                IQueryable<ContractDetail> filter;



                filter = _db.ContractDetails.Where(x => x.ContractId == ContractId).OrderBy(c=>c.Date);
                var entity = _mapper.Map<List<ContractDetailDto>>(filter);

                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = entity
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
        public async Task<ResponseData> GetByIdContractDetail(int id)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                ContractDetail emp = await _db.ContractDetails.Where(a => a.Id == id).FirstOrDefaultAsync();
                var _ContractDetail = _mapper.Map<ContractDetail, ContractDetailDto>(emp);

                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = _ContractDetail
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
        public async Task<ResponseData> DeleteContractDetail(int id)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                ContractDetail emp = await _db.ContractDetails.Where(a => a.Id == id).FirstOrDefaultAsync();
                _db.ContractDetails.Remove(emp);
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
        public ResponseData SaveContractDetail(ContractDetailDto ContractDetail)
        {
            if (ContractDetail.Id == 0 || ContractDetail.Id == null)
            {
                try
                {

                    ContractDetail newRec = new ContractDetail();
                    newRec = _mapper.Map<ContractDetailDto, ContractDetail>(ContractDetail);
                    newRec.Contract = null;
                    newRec.ContractDetailBills = null;
                    _db.ContractDetails.Add(newRec);
                    _db.SaveChanges();
                    return new ResponseData { Message = "تم الحفظ بنجاح", IsSuccess = true };
                }
                catch (Exception ex)
                {
                    throw new NotSupportedException(ex.Message);
                }
            }
            else if (ContractDetail.Id != 0)
            {
                ContractDetail newRec = new ContractDetail();

                newRec = _mapper.Map<ContractDetailDto, ContractDetail>(ContractDetail);

                ContractDetail _newRec = _db.ContractDetails.SingleOrDefault(u => u.Id == ContractDetail.Id);
                if (_newRec == null)
                    throw new KeyNotFoundException("غير موجود في قاعدة البيانات");
                //Mapper.Map(ServicesProvider, servicesProvider);
                try
                {
                    _db.Entry(_newRec).CurrentValues.SetValues(newRec);
                    newRec.Contract = null;
                    _db.ContractDetails.Attach(_newRec);
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
        public ResponseData SaveListContractDetail(List<ContractDetailDto> ContractDetail)
        {

            try
            {

                List<ContractDetail> newRec = new List<ContractDetail>();
                newRec = _mapper.Map<List<ContractDetailDto>, List<ContractDetail>>(ContractDetail);
                //Utilities.ChangeDateTimezone((DateTime)filter.FromDate, LoggedUser.Timezone);
                TimeZoneInfo tzone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                foreach (var item in newRec)
                {

                   item.Date = TimeZoneInfo.ConvertTimeFromUtc(item.Date, TimeZoneInfo.Local);
                }
                _db.ContractDetails.AddRange(newRec);
                _db.SaveChanges();
                return new ResponseData { Message = "تم الحفظ بنجاح", IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new ResponseData { Message = "حدث خطأ", IsSuccess = false };
            }



        }
        //
        public async Task<ResponseData> GetAllInstallmentAlert(ContractDetailDate Contract)
        {
            try
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var filterDate = _db.ContractDetailBills.Include(c => c.ContractDetail).Where(c => c.ContractDetail.ContractId == Contract.ContractId);
                var contractDetailsId = filterDate.Select(c => c.ContractDetailId);
                IQueryable<ContractDetail> filter;

                filter = _db.ContractDetails.Include(c => c.ContractDetailBills).Where(x => x.ContractId == Contract.ContractId && x.Date >= Contract.FromDate && x.Date <= Contract.ToDate && !contractDetailsId.Contains(x.Id));

                var entity = _mapper.Map<List<ContractDetailDto>>(filter);

                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = entity
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
        public async Task<ResponseData> GetAllViewPayInstallments(int ContractId)
        {
            try
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var data =await _db.ViewPayInstallments.Where(x => x.ContractId == ContractId).OrderBy(c=>c.ContractDetailDate).ToListAsync();
                var entity = _mapper.Map<List<ViewPayInstallmentDto>>(data);
                entity[0].PreviousPaid = entity[0].Paid;
                var Paid2 = _db.ContractDetailBills.Where(c => c.ContractDetailId == entity[0].ContractDetailId).Sum(c => c.Paid);
                entity[0].Remainder = (decimal?)(entity[0].Amount - Paid2);

                for (int i = 1; i < entity.Count(); i++)
                {
                    entity[i].PreviousPaid = entity[i-1].Paid;
                   var Paid = _db.ContractDetailBills.Where(c => c.ContractDetailId == entity[i].ContractDetailId).Sum(c => c.Paid);
                    entity[i].Remainder = (decimal?)(entity[i].Amount - Paid);

                }


               

                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = entity
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
        public async Task<ResponseData> GetAllInstallmentOverdue(int ContractId)
        {
            try
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var filterDate = _db.ContractDetailBills.Include(c => c.ContractDetail).Where(c => c.ContractDetail.ContractId == ContractId);
                var contractDetailsId = filterDate.Select(c => c.ContractDetailId);
                IQueryable<ContractDetail> filter;

                filter = _db.ContractDetails.Include(c => c.ContractDetailBills).Where(x => x.ContractId == ContractId && x.Date <= startDate  && !contractDetailsId.Contains(x.Id));

                var entity = _mapper.Map<List<ContractDetailDto>>(filter);

                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = entity
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
        public async Task<ResponseData> GetAllInstallmentNotPaid(int ContractId)
        {
            try
            {
               
                Stopwatch sw = new Stopwatch();
                sw.Start();
              //  var filterDate = _db.ContractDetailBills.Include(c => c.ContractDetail).Where(c => c.ContractDetail.ContractId == ContractId);
              //  var contractDetailsId = filterDate.Select(c => c.ContractDetailId);
                IQueryable<ContractDetail> filter;

                filter = _db.ContractDetails.Include(c => c.ContractDetailBills).OrderBy(c=>c.Date).Where(x => x.ContractId == ContractId/*&& !contractDetailsId.Contains(x.Id)*/);

                var entity = _mapper.Map<List<ContractDetailDto>>(filter);
               
                entity=entity.Distinct().GroupBy(x => x.Id).Select(y => y.First()).ToList();
                foreach (var item in entity)
                {
                    var modelPaid = _db.ContractDetailBills.Where(c => c.ContractDetailId == item.Id).Sum(c => c.Paid);
                    item.Remainder = item.Amount - modelPaid;

                }
                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = entity
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
        public async Task<ResponseData> DeleteAllContractDetail(int id)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var emp = await _db.ContractDetails.Where(a => a.ContractId == id).ToArrayAsync();
                _db.ContractDetails.RemoveRange(emp);
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
    }
}
