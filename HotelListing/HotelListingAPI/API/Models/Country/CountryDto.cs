using HotelListingAPI.API.Models.Hotel;

namespace HotelListingAPI.API.Models.Country
{
    public class CountryDto : BaseCountryDto
    {
        public int Id { get; set; }
        public IList<HotelGetUpdateDto> Hotels { get; set; }
    }
}
