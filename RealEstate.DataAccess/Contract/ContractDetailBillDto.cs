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
        public double? AmountPaid { get; set; }
        public DateTime? Date { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<ContractDetailBill, ContractDetailBillDto>().ReverseMap();
        }
    }
    public class ViewPayInstallmentDto : ICustomMapping
    {
        public int? ContractDetailId { get; set; }
        public string Name { get; set; }
        public DateTime? ContractDetailDate { get; set; }
        public double? Amount { get; set; }
        public int? ContractId { get; set; }
        public bool? IsExtra { get; set; }
        public int? ContractDetailBillId { get; set; }
        public int? Number { get; set; }
        public double? Paid { get; set; }
        public DateTime? ContractDetailBillDate { get; set; }
        public decimal? Remainder { get; set; }
        public double? PreviousPaid { get; set; }  
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<ViewPayInstallment, ViewPayInstallmentDto>().ReverseMap();
        }
    }
}
