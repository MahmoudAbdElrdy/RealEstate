using AutoMapper;
using Mapper;
using RealEstate.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.DataAccess
{
    public class ProjectVisitDto : ICustomMapping
    {
        public int? Id { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; } 
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public bool Visited { get; set; }
        public string Notes { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<ProjectVisit, ProjectVisitDto>()
                .ForMember(x => x.ProjectName, opt => opt.MapFrom(x => x.Project.Name)).ReverseMap();
                
        }
    }
    public class ProjectVisitSearch : PaginationDto
    {
        public int CustomerId { get; set; }
        public int ProjectId { get; set; }
        public DateTime? Date { get; set; }

    }
}
