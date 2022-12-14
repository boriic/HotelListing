using HotelListingAPI.API.Models;
using HotelListingAPI.API.Models.Hotel;

namespace HotelListingAPI.Service.Common.HotelService
{
    public interface IHotelService
    {
        Task<HotelDto> GetAsync(int id);
        Task<List<HotelGetUpdateDto>> GetAllAsync();
        Task<PagedResult<HotelGetUpdateDto>> GetAllAsync(QueryParameters queryParameters);
        Task AddAsync(HotelCreateDto createHotelDto);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, HotelGetUpdateDto updateHotelDto);
        Task<bool> HotelExists(string hotelName);
        Task<bool> HotelExists(int hotelId);
        Task CheckIfOtherHotelWithNameExists(string hotelName, int hotelId);
    }
}
