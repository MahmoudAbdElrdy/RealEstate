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
    public  class QuestionService
    {
       RealEstateContext _db;
        readonly IMapper _mapper;
        public QuestionService(RealEstateContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;

        }
        private BaseSpecifications<Question> Specifications(QuestionSearch search)
        {
            BaseSpecifications<Question> specification = null;

         
            if ((search.CustomerId!=0))
            {
                var name = new BaseSpecifications<Question>(x => x.CustomerId==search.CustomerId);
                specification = specification?.And(name) ?? name;
            }

            if ((search.Date) != null)
            {
                var dateSalary = new BaseSpecifications<Question>(x => x.Date.Date.Month.Equals(search.Date.Value.Date.Month) && x.Date.Date.Year.Equals(search.Date.Value.Date.Year));
                specification = specification?.And(dateSalary) ?? dateSalary;
            }
            if (!string.IsNullOrEmpty(search.Region))
            {
                var name = new BaseSpecifications<Question>(x => x.Region.Contains(search.Region));
                specification = specification?.And(name) ?? name;
            }
            if (specification == null)
                specification = new BaseSpecifications<Question>();
            specification.AddInclude(x => x.Employee);
            specification.isPagingEnabled = true;
            specification.page = search.PageNumber;
            specification.pageSize = search.PageSize;
            return specification;
        }
        public async Task<ResponseData> GetAll(QuestionSearch search)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                IQueryable<Question> filter;

                var specification = Specifications(search);

                filter = _db.Questions.OrderBy(c=>c.Date).Pagtion(specification, out int count);

                //  var entity = _db.Questions.Include(x => x.Department);
                var entity = _mapper.Map<List<QuestionDto>>(filter);

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
                Question emp =await _db.Questions.Where(a => a.Id == id).FirstOrDefaultAsync();
                var _Question = _mapper.Map<Question, QuestionDto>(emp);
               
                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = _Question
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
                Question emp = await _db.Questions.Where(a => a.Id == id).FirstOrDefaultAsync();
                _db.Questions.Remove(emp);
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
        public ResponseData SaveQuestion(QuestionDto Question)
        {
           if (Question.Id == 0|| Question.Id==null)
            {
                try
                {
                    
                    Question newRec = new Question();
                    newRec = _mapper.Map<QuestionDto, Question>(Question);
                    newRec.Customer = null;
                    newRec.Employee = null;
                    //newRec.Date = DateTime.Now;
                    _db.Questions.Add(newRec);
                    _db.SaveChanges();
                    return new ResponseData { Message = "تم الحفظ بنجاح", IsSuccess = true };
                }
                catch (Exception ex)
                {
                    throw new NotSupportedException(ex.Message);
                }
            }
            else if (Question.Id != 0)
            {
                Question newRec = new Question();

                newRec = _mapper.Map<QuestionDto, Question>(Question);

                Question _newRec = _db.Questions.SingleOrDefault(u => u.Id == Question.Id);
                if (_newRec == null)
                    throw new KeyNotFoundException("غير موجود في قاعدة البيانات");
                //Mapper.Map(ServicesProvider, servicesProvider);
                try
                {
                    _db.Entry(_newRec).CurrentValues.SetValues(newRec);
                    newRec.Customer = null;
                    _db.Questions.Attach(_newRec);
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
