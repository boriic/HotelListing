using HotelListingAPI.API.Models.Country;
using HotelListingAPI.DAL.Entities;

namespace HotelListingAPI.Repository.Common.CountryRepository
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Task<CountryDto> GetDetailsAsync(int id);
        Task<List<CountryGetUpdateDto>> GetCountriesByNameAsync(string name);
    }
}
