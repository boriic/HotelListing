using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListingAPI.API.Models.Country;
using HotelListingAPI.CustomExceptionMiddleware.CustomExceptions;
using HotelListingAPI.DAL.Context;
using HotelListingAPI.DAL.Entities;
using HotelListingAPI.Repository.Common.CountryRepository;
using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.Repository
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly HotelListingDbContext _context;
        private readonly ILogger<CountryRepository> _logger;
        private readonly IMapper _mapper;

        public CountryRepository(HotelListingDbContext context, ILogger<CountryRepository> logger, IMapper mapper) : base(context, logger, mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Method asynchronously gets the country including the hotels if they have any
        /// </summary>
        /// <param name="id">Id of the country that you want to find</param>
        /// <returns>CountryDto object</returns>
        /// <exception cref="NotFoundException">This exception will be thrown if country is not found with the given id</exception>
        public async Task<CountryDto> GetDetailsAsync(int id)
        {
            _logger.LogInformation($"(Repository) {nameof(GetDetailsAsync)} ({id})");

            var country = await _context.Countries.Include(x => x.Hotels)
                .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (country == null)
                throw new NotFoundException(nameof(GetDetailsAsync), id);

            return country;
        }

        public async Task<List<CountryGetUpdateDto>> GetCountriesByNameAsync(string name)
        {
            _logger.LogInformation($"(Repository) {nameof(GetCountriesByNameAsync)} ({name})");

            var countries = await _context.Countries.ProjectTo<CountryGetUpdateDto>(_mapper.ConfigurationProvider).Where(x => x.Name.Contains(name)).ToListAsync();

            return countries;

        }
    }
}
