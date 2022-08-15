using HotelListingAPI.DAL.Context;
using HotelListingAPI.Repository.Common;
using HotelListingAPI.Repository.Common.CountryRepository;
using HotelListingAPI.Repository.Common.HotelRepository;

namespace HotelListingAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HotelListingDbContext _context;
        public ICountryRepository CountryRepository { get; }
        public IHotelRepository HotelRepository { get; }

        public UnitOfWork(HotelListingDbContext context, ICountryRepository countryRepository, IHotelRepository hotelRepository)
        {
            _context = context;
            CountryRepository = countryRepository;
            HotelRepository = hotelRepository;
        }
        public async Task Complete()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
