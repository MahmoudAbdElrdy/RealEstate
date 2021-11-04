using AutoMapper;
using Mapper;
using RealEstate.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.DataAccess
{
    public class ProjectDto : ICustomMapping
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int? Floors { get; set; }
        public int? ApartmentNumber { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Project, ProjectDto>().ReverseMap();

        }
    }
    public class ProjectSearch:PaginationDto
    {
     
        public string Name { get; set; }
        public string Address { get; set; }
        public int? Floors { get; set; }
    }
}
