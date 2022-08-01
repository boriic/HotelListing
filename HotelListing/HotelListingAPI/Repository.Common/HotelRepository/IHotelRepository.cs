using HotelListingAPI.DAL.Entities;

namespace HotelListingAPI.Repository.Common.HotelRepository
{
    public interface IHotelRepository : IGenericRepository<Hotel>
    {
        Task<Hotel> GetDetails(int id);
    }
}
