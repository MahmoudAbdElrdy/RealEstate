using AutoMapper;
using Mapper;
using RealEstate.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.DataAccess
{
    public class ProjectUnitDescriptionDto : ICustomMapping
    {
        public int? Id { get; set; }
        public int? ProjectId { get; set; }
        public string Name { get; set; }
        public double? Area { get; set; }
        public int? Kitchen { get; set; }
        public int? Bath { get; set; }
        public int? Room { get; set; }
        public bool? IsBooked { get; set; }
        public int? FlatID { get; set; }
        public int? FloorNumber { get; set; } 
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<ProjectUnitDescription, ProjectUnitDescriptionDto>().ReverseMap();

        }

    }
    public class ReservationDto : ICustomMapping
    {
        public int Id { get; set; }
        public int ProjectUnitDescriptionId { get; set; }
        public int CustomerId { get; set; }
        public ProgramDto program { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Reservation, ReservationDto>().ReverseMap();

        }
    }
    public class ProgramDto : ICustomMapping
    {
        public int? Id { get; set; }
        public int? ProjectUnitDescriptionId { get; set; }
        public string Name { get; set; }
        public double MeterCost { get; set; }
        public double TotalCost { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Program, ProgramDto>().ReverseMap();

        }
    }
}