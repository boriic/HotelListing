using HotelListingAPI.API.Models;

namespace HotelListingAPI.Service.Common.HomeService
{
    public interface IHomeService
    {
        Task ContactMe(ContactMe contactMeDto);
    }
}
