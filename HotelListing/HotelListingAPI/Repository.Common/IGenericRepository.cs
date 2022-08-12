using HotelListingAPI.API.Models;
using System.Linq.Expressions;

namespace HotelListingAPI.Repository.Common
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetAsync(int? id);
        Task<List<T>> GetAllAsync();
        Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);
        Task<T> AddAsync(T entity);
        Task DeleteAsync(int id);
        Task UpdateAsync (T entity);
        Task<bool> Exists(int id);
        Task<T> FindBy(Expression<Func<T, bool>> predicate);
    }
}
