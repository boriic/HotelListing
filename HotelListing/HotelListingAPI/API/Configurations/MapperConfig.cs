using AutoMapper;
using HotelListingAPI.API.Models.Country;
using HotelListingAPI.API.Models.Hotel;
using HotelListingAPI.DAL.Entities;

namespace HotelListingAPI.API.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, CountryCreateDto>().ReverseMap();
            CreateMap<Country, CountryGetDto>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, UpdateCountryDto>().ReverseMap();
            CreateMap<Hotel, HotelDto>().ReverseMap();
        }
    }
}
