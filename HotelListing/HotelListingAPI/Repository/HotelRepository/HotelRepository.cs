using HotelListingAPI.DAL.Context;
using HotelListingAPI.DAL.Entities;
using HotelListingAPI.Repository.Common.HotelRepository;
using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.Repository.HotelRepository
{
    public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
    {
        private readonly HotelListingDbContext _context;
        private readonly ILogger<HotelRepository> _logger;

        public HotelRepository(HotelListingDbContext context, ILogger<HotelRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Hotel> GetDetails(int id)
        {
            return await _context.Hotels.Include(x => x.Country).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
