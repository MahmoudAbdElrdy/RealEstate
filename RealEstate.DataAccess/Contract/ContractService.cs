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
    public class ContractService
    {
        RealEstateContext _db;
        readonly IMapper _mapper;
        ProjectService ProjectService;
        public ContractService(RealEstateContext db, IMapper mapper, ProjectService projectService)
        {
            _db = db;
            _mapper = mapper;
            ProjectService = projectService;


        }
        private BaseSpecifications<Contract> Specifications(ContractSearch search)
        {
            BaseSpecifications<Contract> specification = null;
            specification?.AddInclude(c => c.ProjectUnit);

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
            if (search.ProjectId != null && search.ProjectId > 0)
            {
                var projectId = new BaseSpecifications<Contract>(x => x.ProjectId == search.ProjectId);
                specification = specification?.And(projectId) ?? projectId;
            }
           
            if (search.ProjectId != null && search.ProjectId > 0)
            {
                var projectId = new BaseSpecifications<Contract>(x => x.ProjectId == search.ProjectId);
                specification = specification?.And(projectId) ?? projectId;
            }
            if (search.IsStock != null)
            {
                var isStock = new BaseSpecifications<Contract>(x => x.IsStock == search.IsStock);
                specification = specification?.And(isStock) ?? isStock;
            }
            if ((search.Date) != null)
            {
                var date = new BaseSpecifications<Contract>(x =>  x.Date.Date.Year.Equals(search.Date));
                specification = specification?.And(date) ?? date;
            }
            if (search.Number != null && search.Number > 0)
            {
                var projectId = new BaseSpecifications<Contract>(x => x.ProjectUnit.Number == search.Number);
                specification = specification?.And(projectId) ?? projectId;
            }
            if (search.NumberFloor != null && search.NumberFloor > 0)
            {
                var projectId = new BaseSpecifications<Contract>(x => x.ProjectUnit.FloorNumber == search.NumberFloor);
                specification = specification?.And(projectId) ?? projectId;
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

                filter = _db.Contracts.Include(c=>c.ProjectUnit).Pagtion(specification, out int count);

                //  var entity = _db.Contracts.Include(x => x.Department);
                var entity = _mapper.Map<List<ContractDto>>(filter);
                foreach (var project in entity)
                {
                    if (project.ProjectId > 0)
                        project.ProjectName = _db.Projects.Find(project.ProjectId).Name;
                    project.ContractFile = _db.FileContracts.Where(c=>c.ContractId==project.Id).Select(x => x.FilePath).ToList();

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
                Contract emp = await _db.Contracts.Include(c => c.FileContracts).Include(x => x.ProjectUnit).Where(a => a.Id == id).FirstOrDefaultAsync();

                var _Contract = _mapper.Map<Contract, ContractDto>(emp);
                _Contract.ContractFile = emp?.FileContracts?.Select(x => x.FilePath).ToList();
                _Contract.UnitListDLL = (await ProjectService.GetProjectUnitList(_Contract.ProjectId)).Data;
                if (_Contract.NumberFloor != null || _Contract.NumberFloor > 0)
                    _Contract.UnitDescriptionsDLL = (await ProjectService.GetUnitDescriptionsByUnti(_Contract.ProjectId, (int)_Contract.NumberFloor)).Data;
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
                Contract emp = await _db.Contracts.Include(c => c.FileContracts).Where(a => a.Id == id).FirstOrDefaultAsync();
                if (emp.FileContracts != null)
                    _db.FileContracts.RemoveRange(emp.FileContracts);
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
        public async Task<ResponseData> Getpaid(int? id)
        {
            try
            {

                var result = _db.ContractDetailBills.Include(x => x.ContractDetail).Where(c => c.ContractDetail.ContractId == id).Sum(m => m.Paid);

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
        public async Task<ResponseData> CancellContract(CancelledContractDto cancelledContract)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                CancelledContract cancelled = _db.CancelledContracts.Where(c => c.Id == cancelledContract.Id).FirstOrDefault();
                if (cancelled == null)
                {
                    cancelled = new CancelledContract();
                    Contract emp = await _db.Contracts.Include(c => c.FileContracts).Where(a => a.Id == cancelledContract.ContractId).FirstOrDefaultAsync();
                    var projectName = _db.Projects.Where(c => c.Id == emp.ProjectId).FirstOrDefault();
                    cancelled.Customer = emp.Name;
                    cancelled.Project = projectName.Name;
                    cancelled.Paid = (double)cancelledContract.Paid;
                    cancelled.Back = (double)cancelledContract.Back;
                    cancelled.Date = (DateTime)cancelledContract.Date;
                    if (emp.FileContracts != null)
                        _db.FileContracts.RemoveRange(emp.FileContracts);
                    _db.Contracts.Remove(emp);
                    _db.CancelledContracts.Add(cancelled);
                }
                else
                {
                
                    cancelled.Paid = (double)cancelledContract.Paid;
                    cancelled.Back = (double)cancelledContract.Back;
                    cancelled.Date = (DateTime)cancelledContract.Date;

                }
              
                _db.SaveChanges();
                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Message = "تم الغاء العقد بنجاح"
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

                    Contract?.ContractFile?.ForEach(x => files.Add(new FileContract { FilePath = x }));
                    newRec.FileContracts = files;
                    newRec.ProjectUnit = null;
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

                Contract _newRec = _db.Contracts.Include(x => x.FileContracts).Include(x => x.ProjectUnit).SingleOrDefault(u => u.Id == Contract.Id);
                if (_newRec == null)
                    throw new KeyNotFoundException("غير موجود في قاعدة البيانات");
                //Mapper.Map(ServicesProvider, servicesProvider);
                try
                {
                    if (_newRec.FileContracts != null)
                        _db.FileContracts.RemoveRange(_newRec.FileContracts);
                    var files = new List<FileContract>();

                    Contract?.ContractFile?.ForEach(x => files.Add(new FileContract { FilePath = x, ContractId = Contract.Id }));
                    _newRec.FileContracts = files;
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
        //CancelledContractDto
        private BaseSpecifications<CancelledContract> SpecificationsCancell(CancelledContractDto search)
        {
            BaseSpecifications<CancelledContract> specification = null;


            if (!string.IsNullOrEmpty(search.Customer))
            {
                var name = new BaseSpecifications<CancelledContract>(x => x.Customer.Contains(search.Customer));
                specification = specification?.And(name) ?? name;
            }
            if (!string.IsNullOrEmpty(search.Project))
            {
                var Project = new BaseSpecifications<CancelledContract>(x => x.Project.Contains(search.Project));
                specification = specification?.And(Project) ?? Project;
            }
            if (search.Paid != null)
            {
                var Paid = new BaseSpecifications<CancelledContract>(x => x.Paid == (search.Paid));
                specification = specification?.And(Paid) ?? Paid;
            }
            if (search.Back != null)
            {
                var Back = new BaseSpecifications<CancelledContract>(x => x.Back == (search.Back));
                specification = specification?.And(Back) ?? Back;
            }

            if ((search.Date) != null)
            {
                var date = new BaseSpecifications<CancelledContract>(x => x.Date.Date.Month.Equals(search.Date.Value.Date.Month) && x.Date.Date.Year.Equals(search.Date.Value.Date.Year));
                specification = specification?.And(date) ?? date;
            }

            if (specification == null)
                specification = new BaseSpecifications<CancelledContract>();

            specification.isPagingEnabled = true;
            specification.page = search.PageNumber;
            specification.pageSize = search.PageSize;
            return specification;
        }
        public async Task<ResponseData> GetAllCancelledContracts(CancelledContractDto search)
        {
            try
            {

                Stopwatch sw = new Stopwatch();
                sw.Start();


                //   var  filter = _db.CancelledContracts;
                IQueryable<CancelledContract> filter;

                var specification = SpecificationsCancell(search);

                filter = _db.CancelledContracts.Pagtion(specification, out int count);

                var entity = _mapper.Map<List<CancelledContractDto>>(filter);

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
        public async Task<ResponseData> GetByName(string name)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Contract emp = await _db.Contracts.Include(x=>x.ProjectUnit.ProjectUnitDescription).FirstOrDefaultAsync(x => x.Name.Contains(name));

                var _contract = _mapper.Map<Contract, ContractReportDto>(emp);
                if (_contract.ProjectId > 0)
                {
                   var item = _db.Projects.Find(_contract.ProjectId);
                    _contract.ProjectName = item.Name;
                    _contract.ProjectAddress = item.Address;
                }
                  
                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = _contract
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
        public async Task<ResponseData> GetAllName()
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var  filter = _db.Contracts;
                var entity = _mapper.Map<List<ContractDto>>(filter);     
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
