using HotelListingAPI.API.Models.Country;

namespace HotelListingAPI.API.Models.Hotel
{
    public class HotelDto : BaseHotelDto
    {
        public int Id { get; set; }
        public CountryGetUpdateDto Country { get; set; }
    }
}