using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListingAPI.DAL.Entities;
using AutoMapper;
using HotelListingAPI.API.Models.Country;
using HotelListingAPI.Repository.Common.CountryRepository;
using Microsoft.AspNetCore.Authorization;
using HotelListingAPI.CustomExceptionMiddleware.CustomExceptions;
using System.Linq.Expressions;
using HotelListingAPI.API.Models;

namespace HotelListingAPI.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ILogger<CountryController> _logger;
        private IMapper _mapper;

        public CountryController(IMapper mapper, ICountryRepository countryRepository, ILogger<CountryController> logger)
        {
            _mapper = mapper;
            _countryRepository = countryRepository;
            _logger = logger;
        }

        // GET: api/Country
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<CountryGetUpdateDto>>> GetCountries()
        {
            _logger.LogInformation("(Controller) Trying to fetch all the countries");

            var countries = _mapper.Map<List<CountryGetUpdateDto>>(await _countryRepository.GetAllAsync());

            if (countries == null)
            {
                throw new NotFoundException("No countries found");
            }

            return countries;
        }
        // GET: api/Country/?StartIndex=0&PageSize=25&PageNumber=1
        [HttpGet]
        public async Task<PagedResult<CountryGetUpdateDto>> GetPagedCountries([FromQuery] QueryParameters queryParameters)
        {
            _logger.LogInformation("(Controller) Trying to fetch all the countries with query parameters");

            var pagedCountriesResult = await _countryRepository.GetAllAsync<CountryGetUpdateDto>(queryParameters);

            if (pagedCountriesResult == null)
            {
                throw new NotFoundException("No countries found");
            }

            return pagedCountriesResult;
        }

        // GET: api/Country/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            _logger.LogInformation($"(Controller) Fetching country with id ({id})");

            var country = _mapper.Map<CountryDto>(await _countryRepository.GetDetailsAsync(id));

            if (country == null)
            {
                throw new NotFoundException(nameof(GetCountry), id);
            }

            return country;
        }

        // PUT: api/Country/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCountry(int id, CountryGetUpdateDto updateCountryDto)
        {
            _logger.LogInformation($"(Controller) Trying to update the country with id ({id})");

            throw new BadHttpRequestException($"Object is not valid");

            if (id != updateCountryDto.Id)
            {
                throw new BadHttpRequestException("Invalid id");
            }

            var country = await _countryRepository.GetAsync(id);

            if (country == null)
            {
                throw new NotFoundException(nameof(PutCountry), updateCountryDto.Id);
            }

            _mapper.Map(updateCountryDto, country);

            await _countryRepository.UpdateAsync(country);

            return NoContent();
        }

        // POST: api/Country
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Country>> PostCountry(CountryCreateDto createCountry)
        {
            _logger.LogInformation($"(Controller) Trying to create country");

            throw new BadHttpRequestException($"Object is not valid");

            if (await _countryRepository.FindBy(x => x.Name == createCountry.Name) != null)
            {
                throw new ArgumentException($"Country with name: {createCountry.Name}, already exists");
            }

            var country = _mapper.Map<Country>(createCountry);

            await _countryRepository.AddAsync(country);

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Country/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            _logger.LogInformation($"(Controller) Trying to delete country with id ({id})");

            var country = await _countryRepository.GetAsync(id);

            if (country == null)
            {
                throw new NotFoundException(nameof(DeleteCountry), id);
            }

            await _countryRepository.DeleteAsync(id);

            return NoContent();
        }

        //Contact me form endpoint
    }
}
