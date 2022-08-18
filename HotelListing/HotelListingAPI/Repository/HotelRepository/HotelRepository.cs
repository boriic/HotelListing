using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListingAPI.API.Models.Hotel;
using HotelListingAPI.CustomExceptionMiddleware.CustomExceptions;
using HotelListingAPI.DAL.Context;
using HotelListingAPI.DAL.Entities;
using HotelListingAPI.Repository.Common.HotelRepository;
using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.Repository
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
        /// <summary>
        /// Method asynchronously gets the hotel including the country if they have any
        /// </summary>
        /// <param name="id">Id of the hotel that you want to find</param>
        /// <returns>HotelDto object</returns>
        /// <exception cref="NotFoundException">This exception will be thrown if country is not found with the given id</exception>
        public async Task<HotelDto> GetDetailsAsync(int id)
        {
            _logger.LogInformation($"(Repository) {nameof(GetDetailsAsync)} ({id})");

            var hotel = await _context.Hotels.Include(x => x.Country)
                .ProjectTo<HotelDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (hotel == null)
                throw new NotFoundException(nameof(GetDetailsAsync), id);

            return hotel;
        }
    }
}
