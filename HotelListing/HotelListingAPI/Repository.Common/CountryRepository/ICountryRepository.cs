using HotelListingAPI.DAL.Entities;

namespace HotelListingAPI.Repository.Common.CountryRepository
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Task<Country> GetDetails(int id);    
    }
}
