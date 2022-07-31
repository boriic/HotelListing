using HotelListingAPI.DAL.Entities;

namespace HotelListingAPI.Repository.Common.CountryRepositories
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Task<Country> GetDetails(int id);    
    }
}
