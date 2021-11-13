using AutoMapper;
using Mapper;
using RealEstate.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.DataAccess
{
    public class ContractDto : ICustomMapping
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string NationalNumber { get; set; }
        public string Phone { get; set; }
        public DateTime? Date { get; set; }
        public string Program { get; set; }
        public string Address { get; set; }
        public bool? IsStock { get; set; }
        public double? TotalCost { get; set; }
        public double? MeterCost { get; set; }
        public int? ProjectUnitId { get; set; }
        public string ProjectName { get; set; }
        public double? StockCount { get; set; }
        public double? MetersCount { get; set; }
        public string Notes { get; set; }
        public int ProjectId { get; set; }
        public List<string> ContractFile { get; set; } 
        //    public string[] Files { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Contract, ContractDto>()

              .ReverseMap();
        }
    }
    public class ContractSearch:PaginationDto
    {
        public string Name { get; set; }
        public string NationalNumber { get; set; }
        public string Phone { get; set; }
        public DateTime? Date { get; set; }
        public string Program { get; set; }
        public string Address { get; set; }
        public int? ProjectId { get; set; }
        public bool? IsStock { get; set; }
        public string Notes { get; set; }
    }
}
