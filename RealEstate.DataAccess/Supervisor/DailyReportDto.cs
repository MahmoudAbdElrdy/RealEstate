using AutoMapper;
using Mapper;
using RealEstate.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.DataAccess
{
    public class DailyReportDto : ICustomMapping
    {
        public int? Id { get; set; }
        public int? EmployeeId { get; set; }
        public int? SupervisorId { get; set; }
        public double? Value { get; set; }
        public string EmployeeSubmitted { get; set; }
        public DateTime? Date { get; set; }
        public string Notes { get; set; }
        public string SupervisorName { get; set; }
        public string EmployeeName { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<DailyReport, DailyReportDto>()
                           .ReverseMap();
            configuration.CreateMap<ViewDailyReport, DailyReportDto>()
                          .ReverseMap();

        }
    }
    public class DailyReportSearch : PaginationDto
    {

        public int? Id { get; set; }
        public int? EmployeeId { get; set; }
        public int? SupervisorId { get; set; }
        public double? Value { get; set; }
        public string EmployeeSubmitted { get; set; }
        public DateTime? Date { get; set; }
        public string Notes { get; set; }
        public string SupervisorName { get; set; }
        public string EmployeeName { get; set; }

    }
}