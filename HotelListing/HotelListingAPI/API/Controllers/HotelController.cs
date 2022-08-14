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
            _logger.LogInformation($"(Controller) {nameof(GetHotels)}");

            var hotels = await _hotelRepository.GetAllAsync<HotelGetUpdateDto>();

            return hotels;
        }

        // GET: api/Hotel/?StartIndex=0&PageSize=25&PageNumber=1
        [HttpGet]
        public async Task<PagedResult<HotelGetUpdateDto>> GetPagedHotels([FromQuery] QueryParameters queryParameters)
        {
            _logger.LogInformation($"(Controller) {nameof(GetPagedHotels)}");

            var pagedHotelsResult = await _hotelRepository.GetAllAsync<HotelGetUpdateDto>(queryParameters);

            return pagedHotelsResult;
        }

        // GET: api/Hotel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            _logger.LogInformation($"(Controller) {nameof(GetHotel)} ({id})");

            var hotel = await _hotelRepository.GetDetails(id);

            return hotel;
        }

        // PUT: api/Hotel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelGetUpdateDto hotelDto)
        {
            _logger.LogInformation($"(Controller) {nameof(PutHotel)} ({id})");

            if (id != hotelDto.Id)
            {
                throw new BadHttpRequestException("Invalid id");
            }

            var hotel = await _hotelRepository.GetAsync<HotelGetUpdateDto>(id);

            await _hotelRepository.UpdateAsync(id, hotelDto);

            return NoContent();
        }

        // POST: api/Hotel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HotelCreateDto>> PostHotel(HotelCreateDto hotelDto)
        {
            _logger.LogInformation($"(Controller) {nameof(PostHotel)}");

            //add this to service later as a method that finds hotel by name?
            if (await _hotelRepository.FindBy(x => x.Name == hotelDto.Name) != null)
            {
                throw new ArgumentException($"Hotel with name: {hotelDto.Name}, already exists");
            }

            var hotel = await _hotelRepository.AddAsync<HotelCreateDto, Hotel>(hotelDto);

            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotelDto);
        }

        // DELETE: api/Hotel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            _logger.LogInformation($"(Controller) {nameof(DeleteHotel)} ({id})");

            await _hotelRepository.GetAsync<HotelDto>(id);

            await _hotelRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
