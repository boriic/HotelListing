using AutoMapper;
using HotelListingAPI.DAL.Context;
using HotelListingAPI.Repository.Common;
using HotelListingAPI.Repository.Common.CountryRepository;
using HotelListingAPI.Repository.Common.HotelRepository;

namespace HotelListingAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HotelListingDbContext _context;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IMapper _mapper;

        private ICountryRepository _countryRepository;
        private IHotelRepository _hotelRepository;

        public UnitOfWork(HotelListingDbContext context, ILoggerFactory loggerFactory, IMapper mapper)
        {
            _context = context;
            _loggerFactory = loggerFactory;
            _mapper = mapper;
        }
        public ICountryRepository CountryRepository => _countryRepository ??= new CountryRepository(_context, _loggerFactory.CreateLogger<CountryRepository>(), _mapper);
        public IHotelRepository HotelRepository => _hotelRepository ??= new HotelRepository(_context, _loggerFactory.CreateLogger<HotelRepository>(), _mapper);

        public async Task Complete()
        {
            await _context.SaveChangesAsync();
            Dispose();
        }
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
