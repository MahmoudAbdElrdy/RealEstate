using AutoMapper;
using RealEstate.Data.Models;
using System;
using System.Collections.Generic;
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
    }
}
