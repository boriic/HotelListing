using HotelListingAPI.Repository.Common.CountryRepository;
using HotelListingAPI.Repository.Common.HotelRepository;

namespace HotelListingAPI.Repository.Common
{
    public interface IUnitOfWork : IDisposable
    {
        ICountryRepository CountryRepository { get; }
        IHotelRepository HotelRepository { get; }
        Task Complete();
    }
}
