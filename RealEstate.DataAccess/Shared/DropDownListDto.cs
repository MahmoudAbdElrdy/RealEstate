using AutoMapper;
using Mapper;
using RealEstate.Data.Models;

namespace RealEstate.DataAccess
{
    public class DropDownListDto : ICustomMapping
    {
        public int Value { get; set; }
         public string Text { get; set; }
     
        public void CreateMappings(Profile configuration)
        {
           
            configuration.CreateMap<Department, DropDownListDto>()
                .ForMember(x => x.Value, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Text, opt => opt.MapFrom(x => x.NameAr)).ReverseMap();

            configuration.CreateMap<Project, DropDownListDto>()
               .ForMember(x => x.Value, opt => opt.MapFrom(x => x.Id))
               .ForMember(x => x.Text, opt => opt.MapFrom(x => x.Name)).ReverseMap();

            configuration.CreateMap<Supervisor, DropDownListDto>()
              .ForMember(x => x.Value, opt => opt.MapFrom(x => x.Id))
              .ForMember(x => x.Text, opt => opt.MapFrom(x => x.Name)).ReverseMap();
            configuration.CreateMap<Employee, DropDownListDto>()
            .ForMember(x => x.Value, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.Text, opt => opt.MapFrom(x => x.Name)).ReverseMap();
        }
    }
}
