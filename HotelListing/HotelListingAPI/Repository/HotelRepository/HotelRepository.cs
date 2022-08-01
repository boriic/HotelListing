using HotelListingAPI.DAL.Context;
using HotelListingAPI.DAL.Entities;
using HotelListingAPI.Repository.Common.HotelRepository;
using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.Repository.HotelRepository
{
    public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
    {
        private readonly HotelListingDbContext _context;
        public HotelRepository(HotelListingDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Hotel> GetDetails(int id)
        {
            return await _context.Hotels.Include(x => x.Country).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
