using AutoMapper;
using RealEstate.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace RealEstate.DataAccess
{
    [ScopedService]
    public  class EmployeeService
    {
       RealEstateContext _db;
        readonly IMapper _mapper;
        public EmployeeService(RealEstateContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;

        }
        public ResponseData SaveUser(EmployeeDto user)
        {
            if (user.Id == 0)
            {
                try
                {
                    if (_db.Employees.Any(x => x.Name.Equals(user.Name)&& x.Phone.Equals(user.Phone)))
                    {
                        return new ResponseData { Message = "إسم المستخدم موجود بالفعل", IsSuccess = false };
                    }
                    Employee newRec = new Employee();
                    newRec = _mapper.Map<EmployeeDto, Employee>(user);
                    _db.Employees.Add(newRec);
                    _db.SaveChanges();
                    return new ResponseData { Message = "تم الحفظ بنجاح", IsSuccess = true };
                }
                catch (Exception ex)
                {
                    throw new NotSupportedException(ex.Message);
                }
            }
            else if (user.Id != 0)
            {
                Employee newRec = new Employee();
                newRec = _mapper.Map<EmployeeDto, Employee>(user);

                Employee _newRec = _db.Employees.SingleOrDefault(u => u.Id == user.Id);
                if (_newRec == null)
                    throw new KeyNotFoundException("User Not Found In Database");
                //Mapper.Map(ServicesProvider, servicesProvider);
                try
                {
                    _db.Entry(_newRec).CurrentValues.SetValues(newRec);
                    _db.Employees.Attach(_newRec);
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


        public EmployeeDto GetUser(int UserId)
        {
            Employee user = _db.Employees.Find(UserId);
            var mappedUser = _mapper.Map<Employee, EmployeeDto>(user);
          
            return mappedUser;
        }
    }
}
