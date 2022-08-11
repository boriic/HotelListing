using HotelListingAPI.DAL.Entities;

namespace HotelListingAPI.Repository.Common.CountryRepository
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Task<Country> GetDetailsAsync(int id);  
        Task<Country> GetByNameAsync(string name);
        Task<bool> Exists(string name);    
    }
}
