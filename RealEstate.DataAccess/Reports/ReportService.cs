using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data.Models;
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
                var result = SqlProcedures.GetExtraContrcat(_db,id, contractExtraName);

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
        public async Task<ResponseData> GetCustomerCard(int id, bool IsExtra)
        {
            try
            {
                var result = SqlProcedures.GetCustomerCard(_db,id, IsExtra);

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
        
        public async Task<ResponseData> GetViewCustomerData()
        {
            try
            {
                var result =_db.ViewCustomerData.ToList();

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
        public async Task<ResponseData> GetViewCustomerData(int year)
        {
            try
            {
                var result = _db.ViewCustomerData.Where(c=>c.Date.EndsWith(year.ToString())).ToList();

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
                var result =_db.ViewCancelledContracts.Where(c=>c.Date.EndsWith(year.ToString())).ToList();

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
        public async Task<ResponseData> GetAlert(int id, DateTime from,DateTime to)
        {
            try
            {
                var result = SqlProcedures.GetAlert(_db, id, from,to);
                foreach(var alert in result)
                {
                    alert.FloorNumber =(int) _db.ProjectUnits.FirstOrDefault(c => c.Id == alert.ProjectUnitID)?.FloorNumber;
                    alert.Number =(int) _db.ProjectUnits.FirstOrDefault(c => c.Id == alert.ProjectUnitID)?.Number;
                    alert.Details = $"  رقم الطابق='{ alert.FloorNumber}'{Environment.NewLine}' رقم الوحدة={ alert.Number}'";
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
    }
}
