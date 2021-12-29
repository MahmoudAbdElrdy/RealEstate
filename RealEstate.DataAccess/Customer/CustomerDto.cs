using AutoMapper;
using Mapper;
using RealEstate.Data.Models;
namespace RealEstate.DataAccess
{
  public  class CustomerDto : ICustomMapping
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Referrer { get; set; }
        public string Phone { get; set; }
        public int? CustomerType { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Customer, CustomerDto>().ReverseMap();
        }
    }
    public class CustomerSearch:PaginationDto
    {
        public string Name { get; set; }
        public string Referrer { get; set; }
        public string Phone { get; set; }
    }
}
