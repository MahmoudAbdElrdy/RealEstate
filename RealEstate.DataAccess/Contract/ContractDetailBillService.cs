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
    public class ContractDetailBillService
    {
        RealEstateContext _db;
        readonly IMapper _mapper;
        public ContractDetailBillService(RealEstateContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;

        }
        public async Task<ResponseData> GetAllContractDetailBill(int ContractDetailId)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                IQueryable<ContractDetailBill> filter;



                filter = _db.ContractDetailBills.Where(x => x.ContractDetailId == ContractDetailId);
                var entity = _mapper.Map<List<ContractDetailBillDto>>(filter);

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
        public async Task<ResponseData> GetByIdContractDetailBill(int id)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                ContractDetailBill emp = await _db.ContractDetailBills.Where(a => a.Id == id).FirstOrDefaultAsync();
                var _ContractDetailBill = _mapper.Map<ContractDetailBill, ContractDetailBillDto>(emp);

                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = _ContractDetailBill
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
        public async Task<ResponseData> DeleteContractDetailBill(int id)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                ContractDetailBill emp = await _db.ContractDetailBills.Where(a => a.Id == id).FirstOrDefaultAsync();
                _db.ContractDetailBills.Remove(emp);
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
        public ResponseData SaveContractDetailBill(ContractDetailBillDto ContractDetailBill)
        {
            if (ContractDetailBill.Id == 0 || ContractDetailBill.Id == null)
            {
                try
                {

                    ContractDetailBill newRec = new ContractDetailBill();
                    newRec = _mapper.Map<ContractDetailBillDto, ContractDetailBill>(ContractDetailBill);
                    newRec.ContractDetail = null;
                    
                    _db.ContractDetailBills.Add(newRec);
                    _db.SaveChanges();
                    return new ResponseData { Message = "تم الحفظ بنجاح", IsSuccess = true };
                }
                catch (Exception ex)
                {
                    throw new NotSupportedException(ex.Message);
                }
            }
            else if (ContractDetailBill.Id != 0)
            {
                ContractDetailBill newRec = new ContractDetailBill();

                newRec = _mapper.Map<ContractDetailBillDto, ContractDetailBill>(ContractDetailBill);

                ContractDetailBill _newRec = _db.ContractDetailBills.SingleOrDefault(u => u.Id == ContractDetailBill.Id);
                if (_newRec == null)
                    throw new KeyNotFoundException("غير موجود في قاعدة البيانات");
                //Mapper.Map(ServicesProvider, servicesProvider);
                try
                {
                    _db.Entry(_newRec).CurrentValues.SetValues(newRec);
                    newRec.ContractDetail = null;
                    _db.ContractDetailBills.Attach(_newRec);
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
