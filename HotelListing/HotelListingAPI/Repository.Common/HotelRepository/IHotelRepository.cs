using HotelListingAPI.API.Models.Hotel;
using HotelListingAPI.DAL.Entities;

namespace HotelListingAPI.Repository.Common.HotelRepository
{
    public interface IHotelRepository : IGenericRepository<Hotel>
    {
        Task<HotelDto> GetDetailsAsync(int id);
    }
}
