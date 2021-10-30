using AutoMapper;
using Mapper;
using RealEstate.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.DataAccess
{
    public class QuestionDto : ICustomMapping
    {
        public int? Id { get; set; }
        public int? CustomerId { get; set; } 
        public string CustomerName { get; set; }  
        public string Question1 { get; set; }
        public DateTime? Date { get; set; }
        public string Region { get; set; }
        public int? CustomerType { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Question, QuestionDto>().
            ForMember(x => x.CustomerName, opt => opt.MapFrom(x => x.Customer.Name)).ReverseMap();

        }
    }
    public class QuestionSearch:PaginationDto
    {
        public int? CustomerId { get; set; }
        public DateTime? Date { get; set; }

    }
}
