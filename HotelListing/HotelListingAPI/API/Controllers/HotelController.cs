using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListingAPI.DAL.Context;
using HotelListingAPI.DAL.Entities;
using HotelListingAPI.Repository.Common.HotelRepository;
using AutoMapper;
using HotelListingAPI.API.Models.Hotel;
using Microsoft.AspNetCore.Authorization;
using HotelListingAPI.CustomExceptionMiddleware.CustomExceptions;
using HotelListingAPI.API.Models;

namespace HotelListingAPI.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    public class HotelController : ControllerBase
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly ILogger<HotelController> _logger;
        private IMapper _mapper;

        public HotelController(IMapper mapper, IHotelRepository hotelRepository, ILogger<HotelController> logger)
        {
            _mapper = mapper;
            _hotelRepository = hotelRepository;
            _logger = logger;
        }

        // GET: api/Hotel/getall
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<HotelGetUpdateDto>>> GetHotels()
        {
            _logger.LogInformation("(Controller) Trying to fetch all the hotels");

            var hotels = _mapper.Map<List<HotelGetUpdateDto>>(await _hotelRepository.GetAllAsync());

            if (hotels == null)
            {
                throw new NotFoundException("No hotels found");
            }

            return hotels;
        }

        // GET: api/Hotel/?StartIndex=0&PageSize=25&PageNumber=1
        [HttpGet]
        public async Task<PagedResult<HotelGetUpdateDto>> GetPagedHotels([FromQuery] QueryParameters queryParameters)
        {
            _logger.LogInformation("(Controller) Trying to fetch all the countries with query parameters");

            var pagedHotelsResult = await _hotelRepository.GetAllAsync<HotelGetUpdateDto>(queryParameters);

            if (pagedHotelsResult == null)
            {
                throw new NotFoundException("No hotels found");
            }

            return pagedHotelsResult;
        }

        // GET: api/Hotel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            _logger.LogInformation($"(Controller) Fetching hotel with id ({id})");

            var hotel = _mapper.Map<HotelDto>(await _hotelRepository.GetDetails(id));

            if (hotel == null)
            {
                throw new NotFoundException(nameof(GetHotel), id);
            }

            return hotel;
        }

        // PUT: api/Hotel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelGetUpdateDto hotelDto)
        {
            _logger.LogInformation($"Trying to update the hotel with id ({id})");

            if (!ModelState.IsValid)
                throw new BadHttpRequestException($"(Controller) Object is not valid");

            if (id != hotelDto.Id)
            {
                throw new BadHttpRequestException("Invalid id");
            }

            var hotel = await _hotelRepository.GetAsync(id);

            if (hotel == null)
            {
                throw new NotFoundException(nameof(PutHotel), hotelDto.Id);
            }

            _mapper.Map(hotelDto, hotel);

            await _hotelRepository.UpdateAsync(hotel);

            return NoContent();
        }

        // POST: api/Hotel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(HotelCreateDto hotelDto)
        {
            _logger.LogInformation($"(Controller) Trying to create hotel");

            if (!ModelState.IsValid)
                throw new BadHttpRequestException($"Object is not valid");

            if (await _hotelRepository.FindBy(x => x.Name == hotelDto.Name) != null)
            {
                throw new ArgumentException($"Hotel with name: {hotelDto.Name}, already exists");
            }

            var hotel = _mapper.Map<Hotel>(hotelDto);

            await _hotelRepository.AddAsync(hotel);

            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
        }

        // DELETE: api/Hotel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            _logger.LogInformation($"(Controller) Trying to delete hotel with id ({id})");

            var hotel = await _hotelRepository.GetAsync(id);

            if (hotel == null)
            {
                throw new NotFoundException(nameof(DeleteHotel), id);
            }

            await _hotelRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
