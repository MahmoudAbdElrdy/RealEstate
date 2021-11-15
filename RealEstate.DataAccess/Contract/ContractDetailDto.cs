using AutoMapper;
using Mapper;
using RealEstate.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.DataAccess
{
    public class ContractDetailDto : ICustomMapping
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public double? Amount { get; set; }
        public int? ContractId { get; set; }
        public bool? IsExtra { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<ContractDetail, ContractDetailDto>().ReverseMap();
        }
    }
}
