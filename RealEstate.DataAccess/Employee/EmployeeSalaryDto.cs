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
        public int? Id { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? Date { get; set; }
        public double? Fixed { get; set; } = 0;
        public double? ProductionIncentive { get; set; } = 0;
        public double? Rewards { get; set; } = 0;
        public double? AdvancePayment { get; set; } = 0;
        public double? Sanctions { get; set; } = 0;
        public double? Delays { get; set; } = 0;
        public double? SocialInsurance { get; set; } = 0;
        public double? Holidays { get; set; } = 0;
        public double? Buffet { get; set; } = 0;
        public double? Commission { get; set; } = 0;
        public double? Total { get; set; } = 0;
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<EmployeeSalary, EmployeeSalaryDto>().ReverseMap();
        }
    }
    public class EmployeeSalarySearch : PaginationDto
    {
        public int? EmployeeId { get; set; }
        public DateTime? Date { get; set; }
    }
}
