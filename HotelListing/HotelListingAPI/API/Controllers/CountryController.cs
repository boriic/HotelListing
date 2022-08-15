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
using HotelListingAPI.Service.Common.CountryService;

namespace HotelListingAPI.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly ILogger<CountryController> _logger;

        public CountryController(ICountryService countryService, ILogger<CountryController> logger)
        {
            _countryService = countryService;
            _logger = logger;
        }

        // GET: api/Country
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<CountryGetUpdateDto>>> GetCountries()
        {
            _logger.LogInformation("(Controller) Trying to fetch all the countries");

            var countries = await _countryService.GetAllAsync();

            return countries;
        }
        // GET: api/Country/?StartIndex=0&PageSize=25&PageNumber=1
        [HttpGet]
        public async Task<PagedResult<CountryGetUpdateDto>> GetPagedCountries([FromQuery] QueryParameters queryParameters)
        {
            _logger.LogInformation("(Controller) Trying to fetch all the countries with query parameters");

            var pagedCountriesResult = await _countryService.GetAllAsync(queryParameters);

            return pagedCountriesResult;
        }

        // GET: api/Country/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            _logger.LogInformation($"(Controller) Fetching country with id ({id})");

            var country = await _countryService.GetDetailsAsync(id);

            return country;
        }

        // PUT: api/Country/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCountry(int id, CountryGetUpdateDto updateCountryDto)
        {
            _logger.LogInformation($"(Controller) Trying to update the country with id ({id})");

            if (id != updateCountryDto.Id)
            {
                throw new BadHttpRequestException("Invalid id");
            }

            await _countryService.UpdateAsync(id, updateCountryDto);

            return NoContent();
        }

        // POST: api/Country
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CountryCreateDto>> PostCountry(CountryCreateDto createCountry)
        {
            _logger.LogInformation($"(Controller) Trying to create country");

            if (await _countryService.CountryExists(createCountry.Name))
            {
                throw new ArgumentException($"Country with name: {createCountry.Name}, already exists");
            }

            await _countryService.AddAsync(createCountry);

            return Ok(createCountry);
        }

        // DELETE: api/Country/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            _logger.LogInformation($"(Controller) Trying to delete country with id ({id})");

            await _countryService.DeleteAsync(id);

            return NoContent();
        }

        //Contact me form endpoint
    }
}
