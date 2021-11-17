using AutoMapper;
using Mapper;
using RealEstate.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace RealEstate.DataAccess
{
 public   class ContractDetailBillDto : ICustomMapping
    {
        public int? Id { get; set; }
        public int? ContractDetailId { get; set; }
        public int? Number { get; set; }
        public double? Paid { get; set; }
        public DateTime? Date { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<ContractDetailBill, ContractDetailBillDto>().ReverseMap();
        }
    }
}
