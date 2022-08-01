using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListingAPI.DAL.Entities;
using AutoMapper;
using HotelListingAPI.API.Models.Country;
using HotelListingAPI.Repository.Common.CountryRepository;

namespace HotelListingAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                return NotFound();
            }

            return countries;
        }

        // GET: api/Country/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            if (await _countryRepository.GetAllAsync() == null)
            {
                return NotFound();
            }

            var country = _mapper.Map<CountryDto>(await _countryRepository.GetDetails(id));

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }

        // PUT: api/Country/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, CountryGetUpdateDto updateCountryDto)
        {
            if (id != updateCountryDto.Id)
            {
                return BadRequest("Invalid id");
            }

            var country = await _countryRepository.GetAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            _mapper.Map(updateCountryDto, country);

            try
            {
                await _countryRepository.UpdateAsync(country);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Country
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CountryCreateDto createCountry)
        {
            if (await _countryRepository.GetAllAsync() == null)
            {
                return Problem("Entity set 'HotelListingDbContext.Countries'  is null.");
            }
            var country = _mapper.Map<Country>(createCountry);

            await _countryRepository.AddAsync(country);

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Country/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (await _countryRepository.GetAllAsync() == null)
            {
                return NotFound();
            }
            var country = await _countryRepository.GetAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            await _countryRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await _countryRepository.Exists(id);
        }
    }
}
