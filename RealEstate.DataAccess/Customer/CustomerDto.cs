using AutoMapper;
using Mapper;
using RealEstate.Data.Models;
using System;

namespace RealEstate.DataAccess
{
  public  class CustomerDto : ICustomMapping
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Referrer { get; set; }
        public string Phone { get; set; }
        public int? CustomerType { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Customer, CustomerDto>().ReverseMap();
        }
    }
    public class CustomerSearch:PaginationDto
    {
        public string Name { get; set; }
        public string Referrer { get; set; }
        public string Phone { get; set; }
    }
    public class CustomerReport
    {
        public DateTime? FormDate { get; set; }
        public DateTime? ToDate { get; set; } 
        public string Region { get; set; }
    }
}
