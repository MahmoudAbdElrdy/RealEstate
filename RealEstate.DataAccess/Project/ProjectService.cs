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
    public  class ProjectService
    {
       RealEstateContext _db;
        readonly IMapper _mapper;
        public ProjectService(RealEstateContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;

        }
     
        public async Task<ResponseData> GetAll(ProjectSearch search) 
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                IQueryable<Project> filter;

                BaseSpecifications<Project> specification = null;
               
                if ((search.Address) != null)
                {
                    var addrees = new BaseSpecifications<Project>(x => x.Address.Contains(search.Address));
                    specification = specification?.And(addrees) ?? addrees;
                }
                if ((search.Name) != null)
                {
                    var filed = new BaseSpecifications<Project>(x => x.Name.Contains(search.Name));
                    specification = specification?.And(filed) ?? filed;
                }
                if ((search.Floors) != null&&search.Floors!=0)
                {
                    var filed = new BaseSpecifications<Project>(x => x.Floors==(search.Floors));
                    specification = specification?.And(filed) ?? filed;
                }
                if (specification == null)
                    specification= new BaseSpecifications<Project>();
                specification.isPagingEnabled = true;
                specification.page = search.PageNumber;
                specification.pageSize = search.PageSize;
                filter = _db.Projects.Pagtion(specification, out int count);

              //  var entity = _db.EmployeeSalaries.Include(x => x.Department);
                var entity = _mapper.Map<List<ProjectDto>>(filter);
               
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
                Project emp =await _db.Projects.Where(a => a.Id == id).FirstOrDefaultAsync();
                var _Project = _mapper.Map<Project, ProjectDto>(emp);
                        sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = _Project
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
                Project emp = await _db.Projects.Where(a => a.Id == id).FirstOrDefaultAsync();
                _db.Projects.Remove(emp);
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
        public ResponseData SaveApartmentNumber(ProjectDto Project) 
        {
         
          
          if (Project.Id != 0)
            {
              
                Project _newRec = _db.Projects.SingleOrDefault(u => u.Id == Project.Id);
                if (_newRec == null)
                    throw new KeyNotFoundException("غير موجود في قاعدة البيانات");
                //Mapper.Map(ServicesProvider, servicesProvider);
                try
                {
                    _newRec.ApartmentNumber = Project.ApartmentNumber;
                    _db.Projects.Attach(_newRec);
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
        public ResponseData SaveProject(ProjectDto Project)
        {
         
            if (Project.Id == 0|| Project.Id == null)
            {
                try
                {
                   
                    Project newRec = new Project();
                    newRec = _mapper.Map<ProjectDto, Project>(Project);
                  
                    _db.Projects.Add(newRec);
                    _db.SaveChanges();
                    return new ResponseData { Message = "تم الحفظ بنجاح", IsSuccess = true };
                }
                catch (Exception ex)
                {
                    throw new NotSupportedException(ex.Message);
                }
            }
            else if (Project.Id != 0)
            {
                Project newRec = new Project();

                newRec = _mapper.Map<ProjectDto, Project>(Project);

                Project _newRec = _db.Projects.SingleOrDefault(u => u.Id == Project.Id);
                if (_newRec == null)
                    throw new KeyNotFoundException("غير موجود في قاعدة البيانات");
                //Mapper.Map(ServicesProvider, servicesProvider);
                try
                {
                    _db.Entry(_newRec).CurrentValues.SetValues(newRec);
                    _db.Projects.Attach(_newRec);
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
        public async Task<ResponseData> GetProjectUnitDescriptionById(int FlatID, int projectId) 
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                ProjectUnitDescription emp = await _db.ProjectUnitDescriptions.Where(a => a.FlatID == FlatID && a.ProjectId==projectId).FirstOrDefaultAsync();
                var _Project = _mapper.Map<ProjectUnitDescription, ProjectUnitDescriptionDto>(emp);
                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = _Project
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
        public ResponseData SaveProjectUnitDescription(ProjectUnitDescriptionDto projectUnitDescription) 
        {

            if (projectUnitDescription.Id == 0 || projectUnitDescription.Id == null)
            {
                try
                {

                    ProjectUnitDescription newRec = new ProjectUnitDescription();
                    newRec = _mapper.Map<ProjectUnitDescriptionDto, ProjectUnitDescription>(projectUnitDescription);

                    _db.ProjectUnitDescriptions.Add(newRec);
                    _db.SaveChanges();
                    return new ResponseData { Message = "تم الحفظ بنجاح", IsSuccess = true };
                }
                catch (Exception ex)
                {
                    throw new NotSupportedException(ex.Message);
                }
            }
            else if (projectUnitDescription.Id != 0)
            {
                ProjectUnitDescription newRec = new ProjectUnitDescription();

                newRec = _mapper.Map<ProjectUnitDescriptionDto, ProjectUnitDescription>(projectUnitDescription);

                ProjectUnitDescription _newRec = _db.ProjectUnitDescriptions.SingleOrDefault(u => u.Id == projectUnitDescription.Id);
                if (_newRec == null)
                    throw new KeyNotFoundException("غير موجود في قاعدة البيانات");
                //Mapper.Map(ServicesProvider, servicesProvider);
                try
                {
                    _db.Entry(_newRec).CurrentValues.SetValues(newRec);
                    _db.ProjectUnitDescriptions.Attach(_newRec);
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
