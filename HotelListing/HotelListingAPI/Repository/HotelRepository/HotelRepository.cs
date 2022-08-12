using AutoMapper;
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
        private readonly IMapper _mapper;

        public HotelRepository(HotelListingDbContext context, ILogger<HotelRepository> logger, IMapper mapper) : base(context, logger, mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Hotel> GetDetails(int id)
        {
            return await _context.Hotels.Include(x => x.Country).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
