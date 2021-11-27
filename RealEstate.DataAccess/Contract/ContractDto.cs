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
        public int? NumberFloor { get; set; }
        public List<ProjectUnitDto> UnitDescriptionsDLL { get; set; } 
        public List<ProjectUnitDto> UnitListDLL { get; set; }  

        //    public string[] Files { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Contract, ContractDto>().
                   ForMember(x => x.NumberFloor, opt => opt.MapFrom(x => x.ProjectUnit.FloorNumber))

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
    public class CancelledContractDto : PaginationDto, ICustomMapping
    {
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        public string Project { get; set; }
        public string Customer { get; set; }
        public double? Paid { get; set; }
        public double? Back { get; set; }
        public int? ContractId { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<CancelledContract, CancelledContractDto>().
                     ReverseMap();
        }
    }
    public class ContractReportDto : ICustomMapping
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
   
        public int? Number { get; set; }
        public int? FloorNumber { get; set; }
        public double? Area { get; set; }
        public string ProjectAddress { get; set; } 
        public int? Floors { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Contract, ContractReportDto>().
                   ForMember(x => x.FloorNumber, opt => opt.MapFrom(x => x.ProjectUnit.FloorNumber)).
                   ForMember(x => x.Number, opt => opt.MapFrom(x => x.ProjectUnit.Number)).
                   ForMember(x => x.Area, opt => opt.MapFrom(x => x.ProjectUnit.ProjectUnitDescription.Area)).
                   
                   ReverseMap();
        }
    }
}
