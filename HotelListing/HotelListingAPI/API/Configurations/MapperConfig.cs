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
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, CountryGetUpdateDto>().ReverseMap();
            CreateMap<Country, CountryCreateDto>().ReverseMap();

            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Hotel, HotelGetUpdateDto>().ReverseMap();
            CreateMap<Hotel, HotelCreateDto>().ReverseMap();
        }
    }
}
