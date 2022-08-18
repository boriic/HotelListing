using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using HotelListingAPI.API.Models.Hotel;
using Microsoft.AspNetCore.Authorization;
using HotelListingAPI.API.Models;
using HotelListingAPI.Service.Common.HotelService;

namespace HotelListingAPI.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly ILogger<HotelController> _logger;
        private IMapper _mapper;

        public HotelController(IHotelService hotelService, ILogger<HotelController> logger)
        {
            _hotelService = hotelService;
            _logger = logger;
        }

        // GET: api/v1.0/Hotel/getall
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<HotelGetUpdateDto>>> GetHotels()
        {
            _logger.LogInformation($"(Controller) {nameof(GetHotels)}");

            var hotels = await _hotelService.GetAllAsync();

            return hotels;
        }

        // GET: api/v1.0/Hotel/?StartIndex=0&PageSize=25&PageNumber=1
        [HttpGet]
        public async Task<PagedResult<HotelGetUpdateDto>> GetPagedHotels([FromQuery] QueryParameters queryParameters)
        {
            _logger.LogInformation($"(Controller) {nameof(GetPagedHotels)}");

            var pagedHotelsResult = await _hotelService.GetAllAsync(queryParameters);

            return pagedHotelsResult;
        }

        // GET: api/v1.0/Hotel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            _logger.LogInformation($"(Controller) {nameof(GetHotel)} ({id})");

            var hotel = await _hotelService.GetAsync(id);

            return hotel;
        }

        // PUT: api/v1.0/Hotel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelGetUpdateDto updateHotelDto)
        {
            _logger.LogInformation($"(Controller) {nameof(PutHotel)} ({id})");

            if (id != updateHotelDto.Id)
            {
                throw new BadHttpRequestException("Invalid id");
            }

            await _hotelService.CheckIfOtherHotelWithNameExists(updateHotelDto.Name, id);

            await _hotelService.UpdateAsync(id, updateHotelDto);

            return NoContent();
        }

        // POST: api/v1.0/Hotel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HotelCreateDto>> PostHotel(HotelCreateDto createHotelDto)
        {
            _logger.LogInformation($"(Controller) {nameof(PostHotel)}");

            if (await _hotelService.HotelExists(createHotelDto.Name))
            {
                throw new ArgumentException($"Hotel with name: {createHotelDto.Name}, already exists");
            }

            await _hotelService.AddAsync(createHotelDto);

            return Ok(createHotelDto);
        }

        // DELETE: api/v1.0/Hotel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            _logger.LogInformation($"(Controller) {nameof(DeleteHotel)} ({id})");

            await _hotelService.DeleteAsync(id);

            return NoContent();
        }
    }
}
