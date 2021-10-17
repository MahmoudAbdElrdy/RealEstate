using AutoMapper;
using Mapper;

namespace RealEstate.DataAccess
{
    public class DropDownListDto : ICustomMapping
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public void CreateMappings(Profile configuration)
        {
           
            configuration.CreateMap<EmployeeDto, DropDownListDto>()
                .ForMember(x => x.Key, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Value, opt => opt.MapFrom(x => x.Name)).ReverseMap();
           

        }
    }
}
