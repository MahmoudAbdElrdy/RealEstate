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
    public  class CustomerService
    {
       RealEstateContext _db;
        readonly IMapper _mapper;
        public CustomerService(RealEstateContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;

        }
        private BaseSpecifications<Customer> Specifications(CustomerSearch search)
        {
            BaseSpecifications<Customer> specification = null;

         
            if (!string.IsNullOrEmpty(search.Name))
            {
                var name = new BaseSpecifications<Customer>(x => x.Name.Contains(search.Name));
                specification = specification?.And(name) ?? name;
            }
            if (!string.IsNullOrEmpty(search.Phone))
            {
                var phone = new BaseSpecifications<Customer>(x => x.Phone.Contains(search.Phone));
                specification = specification?.And(phone) ?? phone;
            }
            if (!string.IsNullOrEmpty(search.Referrer))
            {
                var referrer = new BaseSpecifications<Customer>(x => x.Referrer.Contains(search.Referrer));
                specification = specification?.And(referrer) ?? referrer;
            }

            if (specification == null)
                specification = new BaseSpecifications<Customer>();
            
            specification.isPagingEnabled = true;
            specification.page = search.PageNumber;
            specification.pageSize = search.PageSize;
            return specification;
        }
        public async Task<ResponseData> GetAll(CustomerSearch search)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                IQueryable<Customer> filter;

                var specification = Specifications(search);

                filter = _db.Customers.Pagtion(specification, out int count);

                //  var entity = _db.Customers.Include(x => x.Department);
                var entity = _mapper.Map<List<CustomerDto>>(filter);

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
                Customer emp =await _db.Customers.Where(a => a.Id == id).FirstOrDefaultAsync();
                var _Customer = _mapper.Map<Customer, CustomerDto>(emp);
               
                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                return new ResponseData
                {
                    IsSuccess = true,
                    Code = EResponse.OK,
                    Data = _Customer
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
                Customer emp = await _db.Customers.Where(a => a.Id == id).FirstOrDefaultAsync();
                _db.Customers.Remove(emp);
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
        public ResponseData SaveCustomer(CustomerDto Customer)
        {
           if (Customer.Id == 0|| Customer.Id==null)
            {
                try
                {
                    if (_db.Customers.Any(x => x.Name.Equals(Customer.Name)&& x.Phone.Equals(Customer.Phone)))
                    {
                        return new ResponseData { Message = "إسم المستخدم موجود بالفعل", IsSuccess = false };
                    }
                    Customer newRec = new Customer();
                    newRec = _mapper.Map<CustomerDto, Customer>(Customer);

                    _db.Customers.Add(newRec);
                    _db.SaveChanges();
                    return new ResponseData { Message = "تم الحفظ بنجاح", IsSuccess = true };
                }
                catch (Exception ex)
                {
                    throw new NotSupportedException(ex.Message);
                }
            }
            else if (Customer.Id != 0)
            {
                Customer newRec = new Customer();

                newRec = _mapper.Map<CustomerDto, Customer>(Customer);

                Customer _newRec = _db.Customers.SingleOrDefault(u => u.Id == Customer.Id);
                if (_newRec == null)
                    throw new KeyNotFoundException("Customer Not Found In Database");
                //Mapper.Map(ServicesProvider, servicesProvider);
                try
                {
                    _db.Entry(_newRec).CurrentValues.SetValues(newRec);
                    _db.Customers.Attach(_newRec);
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
