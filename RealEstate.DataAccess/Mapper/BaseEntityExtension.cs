using AutoMapper;

namespace Mapper
{
    public static class BaseEntityExtension
    {
        private static IMapper _mapper;

        public static void Configure(IMapper mapper)
        {
            _mapper = mapper;
        }
        public static T MapTo<T>(this T entity)
        {
            return _mapper.Map<T>(entity);
        }
    }
}