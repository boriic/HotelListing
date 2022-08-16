using HotelListingAPI.API.Models;
using HotelListingAPI.Service.Common.HomeService;
using Microsoft.AspNetCore.Mvc;

namespace HotelListingAPI.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _homeService;

        public HomeController(ILogger<HomeController> logger, IHomeService homeService)
        {
            _logger = logger;
            _homeService = homeService;
        }


        // POST: api/v1.0/Home/ContactMe
        [HttpPost("ContactMe")]
        public async Task ContactMe(ContactMe contactMe)
        {
            _logger.LogInformation($"(Controller) {nameof(ContactMe)}");

            await _homeService.ContactMe(contactMe);
        }
    }
}
