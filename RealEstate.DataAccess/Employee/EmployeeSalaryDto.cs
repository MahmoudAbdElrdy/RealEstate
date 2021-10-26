using AutoMapper;
using Mapper;
using RealEstate.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.DataAccess
{
    public class EmployeeSalaryDto : ICustomMapping
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public int? Fixed { get; set; }
        public int? ProductionIncentive { get; set; }
        public int? Rewards { get; set; }
        public int? AdvancePayment { get; set; }
        public int? Sanctions { get; set; }
        public int? Delays { get; set; }
        public int? SocialInsurance { get; set; }
        public int? Holidays { get; set; }
        public int? Buffet { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<EmployeeSalary, EmployeeSalaryDto>().ReverseMap();
        }
    }
}
