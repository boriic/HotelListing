using HotelListingAPI.API.Models;
using System.Linq.Expressions;

namespace HotelListingAPI.Repository.Common
{
    public interface IGenericRepository<T> where T : class
    {
        Task<TResult> GetAsync<TResult>(int? id);
        Task<List<TResult>> GetAllAsync<TResult>();
        Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);
        Task AddAsync<TSource>(TSource source);
        Task DeleteAsync(int id);
        Task UpdateAsync<TSource>(int id,TSource source);
        Task<bool> Exists(int id);
        Task<TResult> FindBy<TResult>(Expression<Func<T, bool>> predicate);
    }
}
