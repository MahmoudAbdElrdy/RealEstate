using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data.Models;
using RealEstate.Data.StoredProc;
using RealEstate.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace RealEstate.DataAccess
{
    [ScopedService]
    public class ReportService
    {
        RealEstateContext _db;
        readonly IMapper _mapper;
        public ReportService(RealEstateContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;

        }
        public async Task<ResponseData> GetExtraContrcat(int id, string contractExtraName)
        {
            try
            {
                var result = SqlProcedures.GetExtraContrcat(_db, id, contractExtraName);
                result = result.Distinct().GroupBy(x => x.ContractDetailId).Select(y => y.First()).ToList();
                var ids = result.Select(x => x.ContractId);
                var dataAll = _db.Contracts.Where(x => x.ProjectId == id&& !ids.Contains(x.Id));

                foreach (var alert in result)
                {
                    alert.Paid = _db.ContractDetailBills.Where(c => c.ContractDetailId == alert.ContractDetailId).Sum(c => c.Paid);
                  //  alert.ContractDetailAmount = alert.ContractDetailAmount - alert.Paid;

                }
                ExtraContrcat extraContrcat = new ExtraContrcat();
                foreach (var alert in dataAll)
                {
                     extraContrcat = new ExtraContrcat();
                    extraContrcat.CustomerName = alert.Name;
                    extraContrcat.Paid = 0;
                    extraContrcat.ContractDetailAmount = 0;
                    result.Add(extraContrcat);


                }
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = result
                };
            }
            catch (Exception ex)
            {

                return new ResponseData
                {
                    IsSuccess = false,
                    Code = EResponse.UnexpectedError,
                    Message = ex.Message,
                };
            }
        }
        public async Task<ResponseData> GetCustomerCard(int id)
        {
            try
            {
                var result = SqlProcedures.GetCustomerCard(_db, id).ToList();
                string name = "";
                foreach (var model in result)
                {
                   
                    var Paid = _db.ContractDetailBills.Where(c => c.ContractDetailId == model.ContractDetailId).Sum(c => c.Paid);
                  
                    model.Remainder = model.Amount - Paid;
                    if (model.AmountPaid == null)
                    {
                        model.AmountPaid = 0;
                    }
                }
                //for(int i = 0; i <= result.Count - 1; i++)
                //{
                //    name = result[i].Name;

                //}
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = result
                };
            }
            catch (Exception ex)
            {

                return new ResponseData
                {
                    IsSuccess = false,
                    Code = EResponse.UnexpectedError,
                    Message = ex.Message,
                };
            }
        }

        public async Task<ResponseData> GetViewCustomerData(int? ProjectId)
        {
            try
            {
                var result = new List<ViewCustomerDatum>();
                if (ProjectId == null|| ProjectId == 0)
                {
                     result = _db.ViewCustomerData.ToList();
                }
                else
                {
                     result = _db.ViewCustomerData.Where(c => c.ProjectId == ProjectId).ToList();
                }
                foreach(var item in result)
                {
                    var unit=_db.ProjectUnits.FirstOrDefault(c => c.Id == item.ProjectUnitId);
                    if (unit != null)
                    {
                        item.Floors = unit?.FloorNumber;
                        item.ApartmentNumber = unit?.Number;

                    }
                    else
                    {
                        item.Floors =0;
                        item.ApartmentNumber = 0;
                    }
                 
                   // alert.Details = $"رقم الطابق={ alert.FloorNumber}{Environment.NewLine}رقم الوحدة={ alert.Number}";

                }

                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = result
                };
            }
            catch (Exception ex)
            {

                return new ResponseData
                {
                    IsSuccess = false,
                    Code = EResponse.UnexpectedError,
                    Message = ex.Message,
                };
            }
        }
        public async Task<ResponseData> GetViewCustomerDatayear(int year)
        {
            try
            {
                var result = _db.ViewCustomerData.Where(c => c.Date.EndsWith(year.ToString())).ToList();

                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = result
                };
            }
            catch (Exception ex)
            {

                return new ResponseData
                {
                    IsSuccess = false,
                    Code = EResponse.UnexpectedError,
                    Message = ex.Message,
                };
            }
        }
        public async Task<ResponseData> GetViewCancelledContract(int year)
        {
            try
            {
                var result = _db.ViewCancelledContracts.Where(c => c.Date.EndsWith(year.ToString())).ToList();

                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = result
                };
            }
            catch (Exception ex)
            {

                return new ResponseData
                {
                    IsSuccess = false,
                    Code = EResponse.UnexpectedError,
                    Message = ex.Message,
                };
            }
        }
        public async Task<ResponseData> GetAlert(int id, DateTime from, DateTime to)
        {
            try
            {
               
                var result = SqlProcedures.GetAlert(_db, id, from, to).Where(x=>x.ProjectUnitID!=null).OrderByDescending(c=>c.CustomerName).ToList();
                foreach (var alert in result)
                {
                    if (alert.ProjectUnitID != null)
                    {
                        alert.FloorNumber = (int)_db.ProjectUnits.FirstOrDefault(c => c.Id == alert.ProjectUnitID)?.FloorNumber;
                        alert.Number = (int)_db.ProjectUnits.FirstOrDefault(c => c.Id == alert.ProjectUnitID)?.Number;
                        alert.Details = $"رقم الطابق={ alert.FloorNumber}{Environment.NewLine}رقم الوحدة={ alert.Number}";
                    }
                  
                    alert.CustomerName = $"{ alert.CustomerName}{Environment.NewLine}ت:{ alert.CustomerPhone}";
                    if (alert.ContractDetailId != null)
                    {
                        if (alert.Paid == null)
                            alert.Paid = 0;
                        var itemlist = _db.ContractDetailBills.Where(c => c.ContractDetailId == alert.ContractDetailId);
                        if (itemlist.Count() > 0)
                        {
                            alert.Paid = itemlist.Sum(c => c.Paid);


                        }
                        if (alert.Remainder == null)
                            alert.Remainder = 0;
                        if (alert.Amount == null)
                            alert.Amount = 0;
                        alert.Remainder = (double)(alert.Amount - alert.Paid);
                    }
                  

                }
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = result.Where(c=>c.Remainder!=0)
                };
            }
            catch (Exception ex)
            {

                return new ResponseData
                {
                    IsSuccess = false,
                    Code = EResponse.UnexpectedError,
                    Message = ex.Message,
                };
            }
        } 
        
        public async Task<ResponseData> GetOverdue(int ProjectId)
        {
            try
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
                var result = SqlProcedures.GetOverdue(_db, ProjectId, now);
                var contrcatDatiesIDs = result.Select(c=>c.ContractID);
                var filterDate = _db.ContractDetailBills.Where(c => contrcatDatiesIDs.Contains(c.ContractDetailId));

              //  var filterDate = _db.ContractDetailBills.Include(c => c.ContractDetail.Contract).Where(c => c.ContractDetail.Contract.ProjectId == ProjectId&&c.Date<=now);
                var contractDetailsId = filterDate.Select(c => c.ContractDetailId).ToList();
              
              
                result = result.Where(x => !contractDetailsId.Contains((int)x.ContractID)).ToList();
                //foreach (var alert in result)
                //{
                //    alert.FloorNumber = (int)_db.ProjectUnits.FirstOrDefault(c => c.Id == alert.ProjectUnitID)?.FloorNumber;
                //    alert.Number = (int)_db.ProjectUnits.FirstOrDefault(c => c.Id == alert.ProjectUnitID)?.Number;
                //    alert.Details = $"رقم الطابق={ alert.FloorNumber}{Environment.NewLine}رقم الوحدة={ alert.Number}";
                //    alert.CustomerName = $"{ alert.CustomerName}{Environment.NewLine}ت:{ alert.CustomerPhone}";
                //    alert.Paid = _db.ContractDetailBills.Where(c => c.ContractDetailId == alert.ContractDetailId).Sum(c => c.Paid);
                //    alert.Remainder = (double)(alert.Amount-alert.Paid);

                //}
                foreach (var alert in result)
                {
                    if (alert.ProjectUnitID != null)
                    {
                        alert.FloorNumber = (int)_db.ProjectUnits.FirstOrDefault(c => c.Id == alert.ProjectUnitID)?.FloorNumber;
                        alert.Number = (int)_db.ProjectUnits.FirstOrDefault(c => c.Id == alert.ProjectUnitID)?.Number;
                        alert.Details = $"رقم الطابق={ alert.FloorNumber}{Environment.NewLine}رقم الوحدة={ alert.Number}";
                    }

                    alert.CustomerName = $"{ alert.CustomerName}{Environment.NewLine}ت:{ alert.CustomerPhone}";
                    if (alert.ContractDetailId != null)
                    {
                        if (alert.Paid == null)
                            alert.Paid = 0;
                        var itemlist = _db.ContractDetailBills.Where(c => c.ContractDetailId == alert.ContractDetailId);
                        if (itemlist.Count() > 0)
                        {
                            alert.Paid = itemlist.Sum(c => c.Paid);


                        }
                        if (alert.Remainder == null)
                            alert.Remainder = 0;
                        if (alert.Amount == null)
                            alert.Amount = 0;
                        alert.Remainder = (double)(alert.Amount - alert.Paid);
                    }


                }
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = result.Where(c=>c.Remainder!=0)
                };
            }
            catch (Exception ex)
            {

                return new ResponseData
                {
                    IsSuccess = false,
                    Code = EResponse.UnexpectedError,
                    Message = ex.Message,
                };
            }
        }
        public async Task<ResponseData> GetPrintBill(int id)
        {
            try
            {

                var result = SqlProcedures.GetPrintBill(_db, id);
                foreach (var alert in result)
                {
                     alert.Paid = (decimal)_db.ContractDetailBills.Where(c => c.ContractDetailId == alert.ContractDetailId).Sum(c => c.Paid);
                     alert.Remainder = (decimal)alert.Amount - alert.Paid;

                }
                var customer= _db.ViewCustomerData.Where(c => c.Id == id).FirstOrDefault();
                result = result.Where(c => c.Remainder != 0).ToList();
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = result,
                    Data2= customer
                };
            }
            catch (Exception ex)
            {

                return new ResponseData
                {
                    IsSuccess = false,
                    Code = EResponse.UnexpectedError,
                    Message = ex.Message,
                };
            }
        }
        public async Task<ResponseData> Getpaid(int id) 
        {
            try
            {

                var result =_db.ContractDetailBills.Include(x=>x.ContractDetail).Where(c=>c.ContractDetail.ContractId==id).Sum(m=>m.Paid);
               
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = result
                };
            }
            catch (Exception ex)
            {

                return new ResponseData
                {
                    IsSuccess = false,
                    Code = EResponse.UnexpectedError,
                    Message = ex.Message,
                };
            }
        }
        
        public async Task<ResponseData> GetCustomerReport(CustomerReport search)
        {
            try
            {
                IQueryable<Customer> filter;

               var questions = _db.Questions.Where(c=>c.CustomerType==3);
                if(!string.IsNullOrEmpty(search.Region))
                {
                     questions = questions.Where(c=>c.Region.Contains(search.Region));
                }
                if (search.FormDate!=null)
                {
                    questions = questions.Where(c => c.Date>=search.FormDate);
                }
                if (search.ToDate!=null)
                {
                    questions = questions.Where(c => c.Date <= search.ToDate);
                }
                filter = questions.Select(c => c.Customer).Distinct();
                 var entity = _mapper.Map<List<CustomerDto>>(filter);
               
               
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
        public async Task<ResponseData> GetSupervisorReport(SupervisorReport search)
        {
            try
            {
                IQueryable<SupervisorDetail> supervisorDetail;
                List<ViewDailyReport> dailyReports;
                supervisorDetail = _db.SupervisorDetails;
                dailyReports = _db.ViewDailyReports.ToList();
                if (search.SupervisorId != null && search.SupervisorId != 0)
                {
                    supervisorDetail = supervisorDetail.Where(c => c.SupervisorId == search.SupervisorId);
                    dailyReports = dailyReports.Where(c => c.SupervisorId == search.SupervisorId).ToList();
                }
            

                if (search.FromDate != null)
                {
                    supervisorDetail = supervisorDetail.Where(c => c.Date >= search.FromDate);
                }
                if (search.ToDate != null)
                {
                    supervisorDetail = supervisorDetail.Where(c => c.Date <= search.ToDate);
                }
               // filter = supervisorDetail.Select(c => c.Customer).Distinct();
                var entity = _mapper.Map<List<SupervisorDetailDto>>(supervisorDetail);
             

                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = entity,
                    Data2= dailyReports

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
        public async Task<ResponseData> GetSupervisor(int id)
        {
            try
            {

                var result = _db.Supervisors.FirstOrDefault(c=>c.Id==id)?.Name;

                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = result
                };
            }
            catch (Exception ex)
            {

                return new ResponseData
                {
                    IsSuccess = false,
                    Code = EResponse.UnexpectedError,
                    Message = ex.Message,
                };
            }
        }
    }
}
