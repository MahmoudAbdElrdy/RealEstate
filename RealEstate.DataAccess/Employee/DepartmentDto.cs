using AutoMapper;
using Mapper;
using RealEstate.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.DataAccess
{
   public class DepartmentDto : ICustomMapping
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Department, EmployeeDto>().ReverseMap();
        }
    }
}
