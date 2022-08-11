using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListingAPI.DAL.Entities;
using AutoMapper;
using HotelListingAPI.API.Models.Country;
using HotelListingAPI.Repository.Common.CountryRepository;
using Microsoft.AspNetCore.Authorization;
using HotelListingAPI.CustomExceptionMiddleware.CustomExceptions;

namespace HotelListingAPI.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private IMapper _mapper;

        public CountryController(IMapper mapper, ICountryRepository countryRepository)
        {
            _mapper = mapper;
            _countryRepository = countryRepository;
        }

        // GET: api/Country
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryGetUpdateDto>>> GetCountries()
        {

            var countries = _mapper.Map<List<CountryGetUpdateDto>>(await _countryRepository.GetAllAsync());

            if (countries == null)
            {
                throw new NotFoundException("No countries found");
            }

            return countries;
        }

        // GET: api/Country/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
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
            if (await CountryExists(createCountry.Name))
            {
                throw new ArgumentException("Country with that name already exists");
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
            var country = await _countryRepository.GetAsync(id);

            if (country == null)
            {
                throw new NotFoundException(nameof(DeleteCountry), id);
            }

            await _countryRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await _countryRepository.Exists(id);
        }

        private async Task<bool> CountryExists(string name)
        {
            return await _countryRepository.Exists(name);
        }
    }
}
