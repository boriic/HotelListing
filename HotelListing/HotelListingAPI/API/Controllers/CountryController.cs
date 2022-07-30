using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListingAPI.DAL.Context;
using HotelListingAPI.DAL.Entities;
using AutoMapper;
using HotelListingAPI.API.Models.Country;

namespace HotelListingAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly HotelListingDbContext _context;
        private IMapper _mapper;

        public CountryController(HotelListingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Country
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryGetDto>>> GetCountries()
        {

            var countries = _mapper.Map<List<CountryGetDto>>(await _context.Countries.ToListAsync());

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
            if (_context.Countries == null)
            {
                return NotFound();
            }

            var country = _mapper.Map<CountryDto>(await _context.Countries.Include(x => x.Hotels).FirstOrDefaultAsync(x => x.Id == id));

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }

        // PUT: api/Country/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
        {
            if (id != updateCountryDto.Id)
            {
                return BadRequest("Invalid id");
            }

            var country = await _context.Countries.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            _mapper.Map(updateCountryDto, country);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CountryCreateDto createCountry)
        {
            if (_context.Countries == null)
            {
                return Problem("Entity set 'HotelListingDbContext.Countries'  is null.");
            }
            var country = _mapper.Map<Country>(createCountry);
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Country/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (_context.Countries == null)
            {
                return NotFound();
            }
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(int id)
        {
            return (_context.Countries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
