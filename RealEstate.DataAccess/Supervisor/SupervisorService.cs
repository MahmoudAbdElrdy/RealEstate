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
    public class SupervisorService
    {
        RealEstateContext _db;
        readonly IMapper _mapper;
        public SupervisorService(RealEstateContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;

        }
        private BaseSpecifications<Supervisor> Specifications(SupervisorSearch search)
        {
            BaseSpecifications<Supervisor> specification = null;

            if (!string.IsNullOrEmpty(search.Name))
            {
                var name = new BaseSpecifications<Supervisor>(x => x.Name.Contains(search.Name));
                specification = specification?.And(name) ?? name;
            }
            if (!string.IsNullOrEmpty(search.Phone))
            {
                var phone = new BaseSpecifications<Supervisor>(x => x.Phone.Contains(search.Phone));
                specification = specification?.And(phone) ?? phone;
            }
            if (!string.IsNullOrEmpty(search.Job))
            {
                var referrer = new BaseSpecifications<Supervisor>(x => x.Job.Contains(search.Job));
                specification = specification?.And(referrer) ?? referrer;
            }


            if (specification == null)
                specification = new BaseSpecifications<Supervisor>();
           
            specification.isPagingEnabled = true;
            specification.page = search.PageNumber;
            specification.pageSize = search.PageSize;
            return specification;
        }
        public async Task<ResponseData> GetAll(SupervisorSearch search)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                IQueryable<Supervisor> filter;

                var specification = Specifications(search);

                filter = _db.Supervisors.Pagtion(specification, out int count);

                //  var entity = _db.Supervisors.Include(x => x.Department);

                var entity = _mapper.Map<List<SupervisorDto>>(filter);
               foreach(var item in entity)
                {
                    item.Credit = _db.SupervisorDetails.Where(c=>c.SupervisorId==item.Id).Sum(c => c.Credit);
                    item.Debt = _db.SupervisorDetails.Where(c=>c.SupervisorId==item.Id).Sum(c => c.Debt);
                    item.Net = _db.SupervisorDetails.Where(c=>c.SupervisorId==item.Id).Sum(c => c.Net);
                }
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
                Supervisor emp = await _db.Supervisors.Where(a => a.Id == id).FirstOrDefaultAsync();
                var _Supervisor = _mapper.Map<Supervisor, SupervisorDto>(emp);

                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = _Supervisor
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
                Supervisor emp = await _db.Supervisors.Where(a => a.Id == id).FirstOrDefaultAsync();
                _db.Supervisors.Remove(emp);
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
        public ResponseData SaveSupervisor(SupervisorDto Supervisor)
        {
            if (Supervisor.Id == 0 || Supervisor.Id == null)
            {
                try
                {

                    Supervisor newRec = new Supervisor();
                    newRec = _mapper.Map<SupervisorDto, Supervisor>(Supervisor);
                    
                    _db.Supervisors.Add(newRec);
                    _db.SaveChanges();
                    return new ResponseData { Message = "تم الحفظ بنجاح", IsSuccess = true };
                }
                catch (Exception ex)
                {
                    throw new NotSupportedException(ex.Message);
                }
            }
            else if (Supervisor.Id != 0)
            {
                Supervisor newRec = new Supervisor();

                newRec = _mapper.Map<SupervisorDto, Supervisor>(Supervisor);

                Supervisor _newRec = _db.Supervisors.SingleOrDefault(u => u.Id == Supervisor.Id);
                if (_newRec == null)
                    throw new KeyNotFoundException("غير موجود في قاعدة البيانات");
                //Mapper.Map(ServicesProvider, servicesProvider);
                try
                {
                    _db.Entry(_newRec).CurrentValues.SetValues(newRec);

                    _db.Supervisors.Attach(_newRec);
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
                var dropDownList = _db.Supervisors;

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
        public ResponseData SaveSupervisorDetail(SupervisorDetailDto Supervisor)
        {
            if (Supervisor.Id == 0 || Supervisor.Id == null)
            {
                try
                {

                    SupervisorDetail newRec = new SupervisorDetail();
                    newRec = _mapper.Map<SupervisorDetailDto, SupervisorDetail>(Supervisor);
                    newRec.Date = DateTime.Now;
                    _db.SupervisorDetails.Add(newRec);
                    _db.SaveChanges();
                    return new ResponseData { Message = "تم الحفظ بنجاح", IsSuccess = true };
                }
                catch (Exception ex)
                {

                    return new ResponseData { Message = "حدث خطأ", IsSuccess = false };
                }
            }

            return new ResponseData { Message = "حدث خطأ", IsSuccess = false };
        }
    }
}
