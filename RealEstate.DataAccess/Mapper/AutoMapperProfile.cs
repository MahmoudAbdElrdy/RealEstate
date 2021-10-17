using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using RealEstate.DataAccess;

namespace Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            LoadStandardMappings();
            LoadCustomMappings();
            LoadConverters();
        }

        private void LoadConverters()
        {

        }

        private void LoadStandardMappings()
        {
            var mapsFrom = MapperProfileHelper.LoadStandardMappings(Assembly.GetExecutingAssembly());

            foreach (var map in mapsFrom)
            {
                CreateMap(map.Source, map.Destination).ReverseMap();
            }
        }

        private void LoadCustomMappings()
        {
            var assemblies = new List<Assembly>() { (typeof(EmployeeDto).Assembly) };
            var mapsFrom = MapperProfileHelper.LoadCustomMappings(assemblies);
            foreach (var map in mapsFrom)
            {
                map.CreateMappings(this);
            }
        }
    }
}
