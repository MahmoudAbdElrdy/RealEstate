using AutoMapper;
using Mapper;
using RealEstate.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealEstate.DataAccess
{
    public class SupervisorDto : ICustomMapping
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string NationalNumber { get; set; }
        public string Job { get; set; }
        public double? Credit { get; set; }
        public double? Debt { get; set; }
        public double? Net { get; set; }
        public DateTime? Date { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Supervisor, SupervisorDto>()
                           .ReverseMap();
            configuration.CreateMap<ViewSupervisor, SupervisorDto>()
                          .ReverseMap();

        }
    }
    public class SupervisorSearch:PaginationDto
    {
       
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Job { get; set; }

    }
    public class SupervisorReport
    {

        public int? SupervisorId { get; set; }
        public DateTime? FromDate { get; set; } 
        public DateTime? ToDate { get; set; } 

    }
    public class SupervisorDetailDto: ICustomMapping
    {
        public int? Id { get; set; }
        public int? SupervisorId { get; set; }
        public double? Credit { get; set; }
        public double? Debt { get; set; }
        public double? Net { get; set; }
        public DateTime? Date { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<SupervisorDetail, SupervisorDetailDto>().ReverseMap();

        }
    }
}