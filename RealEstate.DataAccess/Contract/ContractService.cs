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
    public  class ContractService
    {
        RealEstateContext _db;
        readonly IMapper _mapper;
        public ContractService(RealEstateContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;

        }
        private BaseSpecifications<Contract> Specifications(ContractSearch search)
        {
            BaseSpecifications<Contract> specification = null;


            if (!string.IsNullOrEmpty(search.Name))
            {
                var name = new BaseSpecifications<Contract>(x => x.Name.Contains(search.Name));
                specification = specification?.And(name) ?? name;
            }
            if (!string.IsNullOrEmpty(search.Notes))
            {
                var note = new BaseSpecifications<Contract>(x => x.Notes.Contains(search.Notes));
                specification = specification?.And(note) ?? note;
            }
            if (!string.IsNullOrEmpty(search.Phone))
            {
                var phone = new BaseSpecifications<Contract>(x => x.Phone.Contains(search.Phone));
                specification = specification?.And(phone) ?? phone;
            }
            if (!string.IsNullOrEmpty(search.Address))
            {
                var Address = new BaseSpecifications<Contract>(x => x.Address.Contains(search.Address));
                specification = specification?.And(Address) ?? Address;
            }
            if (!string.IsNullOrEmpty(search.Program))
            {
                var program = new BaseSpecifications<Contract>(x => x.Program.Contains(search.Program));
                specification = specification?.And(program) ?? program;
            }
            if (!string.IsNullOrEmpty(search.NationalNumber))
            {
                var nationalNumber = new BaseSpecifications<Contract>(x => x.NationalNumber.Contains(search.NationalNumber));
                specification = specification?.And(nationalNumber) ?? nationalNumber;
            }
            if (search.ProjectId!=null&& search.ProjectId>0)
            {
                var projectId = new BaseSpecifications<Contract>(x => x.ProjectId==search.ProjectId);
                specification = specification?.And(projectId) ?? projectId;
            }
            if (search.IsStock != null)
            {
                var isStock = new BaseSpecifications<Contract>(x => x.IsStock == search.IsStock);
                specification = specification?.And(isStock) ?? isStock;
            }
            if ((search.Date) != null)
            {
                var date = new BaseSpecifications<Contract>(x => x.Date.Date.Month.Equals(search.Date.Value.Date.Month) && x.Date.Date.Year.Equals(search.Date.Value.Date.Year));
                specification = specification?.And(date) ?? date;
            }
           
            if (specification == null)
                specification = new BaseSpecifications<Contract>();

            specification.isPagingEnabled = true;
            specification.page = search.PageNumber;
            specification.pageSize = search.PageSize;
            return specification;
        }
        public async Task<ResponseData> GetAll(ContractSearch search)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                IQueryable<Contract> filter;

                var specification = Specifications(search);

                filter = _db.Contracts.Pagtion(specification, out int count);

                //  var entity = _db.Contracts.Include(x => x.Department);
                var entity = _mapper.Map<List<ContractDto>>(filter);
                foreach(var project in entity)
                {
                    if(project.ProjectId>0)
                    project.ProjectName = _db.Projects.Find(project.ProjectId).Name;
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
                Contract emp = await _db.Contracts.Where(a => a.Id == id).FirstOrDefaultAsync();
                var _Contract = _mapper.Map<Contract, ContractDto>(emp);

                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = _Contract
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
                Contract emp = await _db.Contracts.Where(a => a.Id == id).FirstOrDefaultAsync();
                _db.Contracts.Remove(emp);
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
       
        public ResponseData SaveContract(ContractDto Contract)
        {
            if (Contract.Id == 0 || Contract.Id == null)
            {
                try
                {
                    if (_db.Contracts.Any(x => x.Name.Equals(Contract.Name) && x.Phone.Equals(Contract.Phone)))
                    {
                        return new ResponseData { Message = "إسم المستخدم موجود بالفعل", IsSuccess = false };
                    }
                    Contract newRec = new Contract();
                    newRec = _mapper.Map<ContractDto, Contract>(Contract);
                    var files = new List<FileContract>();

                    Contract.ContractFile.ForEach(x => files.Add(new FileContract {  FilePath = x }));
                    newRec.FileContracts = files;
                    _db.Contracts.Add(newRec);
                    _db.SaveChanges();
                    return new ResponseData { Message = "تم الحفظ بنجاح", IsSuccess = true };
                }
                catch (Exception ex)
                {
                    throw new NotSupportedException(ex.Message);
                }
            }
            else if (Contract.Id != 0)
            {
                Contract newRec = new Contract();

                newRec = _mapper.Map<ContractDto, Contract>(Contract);

                Contract _newRec = _db.Contracts.SingleOrDefault(u => u.Id == Contract.Id);
                if (_newRec == null)
                    throw new KeyNotFoundException("غير موجود في قاعدة البيانات");
                //Mapper.Map(ServicesProvider, servicesProvider);
                try
                {
                    _db.Entry(_newRec).CurrentValues.SetValues(newRec);
                    _db.Contracts.Attach(_newRec);
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
