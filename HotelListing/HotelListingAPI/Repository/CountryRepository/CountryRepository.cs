using HotelListingAPI.DAL.Context;
using HotelListingAPI.DAL.Entities;
using HotelListingAPI.Repository.Common.CountryRepository;
using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.Repository.CountryRepository
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly HotelListingDbContext _context;
        private readonly ILogger<CountryRepository> _logger;

        public CountryRepository(HotelListingDbContext context, ILogger<CountryRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Exists(string name)
        {
            var entity = await GetByNameAsync(name);

            return entity != null;
        }

        public async Task<Country> GetByNameAsync(string name)
        {
            return await _context.Countries.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<Country> GetDetailsAsync(int id)
        {
            return await _context.Countries.Include(x => x.Hotels).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
