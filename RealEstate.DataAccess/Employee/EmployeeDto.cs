using AutoMapper;
using Mapper;
using RealEstate.Data.Models;
using System;


namespace RealEstate.DataAccess
{
    public class EmployeeDto : ICustomMapping
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public DateTime? WorkSince { get; set; }
        public string Phone { get; set; }
        public string PassWord { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Employee, EmployeeDto>().ReverseMap();
        }
    }
    public class EmployeeInfo 
    {
        public string  Name { get; set; }

        public string PassWord { get; set; } 

    }
}
