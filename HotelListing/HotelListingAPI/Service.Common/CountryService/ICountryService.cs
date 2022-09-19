using HotelListingAPI.API.Models;
using HotelListingAPI.API.Models.Country;
using HotelListingAPI.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HotelListingAPI.Service.Common.CountryService
{
    public interface ICountryService
    {
        Task<CountryDto> GetAsync(int id);
        Task<List<CountryGetUpdateDto>> GetAllAsync();
        Task<PagedResult<CountryGetUpdateDto>> GetAllAsync(QueryParameters queryParameters);
        Task AddAsync(CountryCreateDto createCountryDto);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, CountryGetUpdateDto updateCountryDto);
        Task<bool> CountryExists(string countryName);
        Task<bool> CountryExists(int countryId);
        Task CheckIfOtherCountryWithNameExists(string countryName, int countryId);
        Task<List<CountryGetUpdateDto>> GetCountriesByNameAsync(string countryName);
    }
}
