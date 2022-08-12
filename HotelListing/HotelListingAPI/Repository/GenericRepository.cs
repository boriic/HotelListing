using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListingAPI.API.Models;
using HotelListingAPI.DAL.Context;
using HotelListingAPI.Repository.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelListingAPI.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelListingDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GenericRepository(HotelListingDbContext context, ILogger logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<T> AddAsync(T entity)
        {
            _logger.LogInformation($"(Repository) {nameof(AddAsync)}");

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation($"(Repository) {nameof(DeleteAsync)}");


            var entity = await GetAsync(id);

            _context.Set<T>().Remove(entity);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id);

            return entity != null;
        }

        public async Task<List<T>> GetAllAsync()
        {
            _logger.LogInformation($"(Repository) {nameof(GetAllAsync)}");


            return await _context.Set<T>().ToListAsync();
        }

        public async Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)
        {
            var totalSize = await _context.Set<T>().CountAsync();

            var items = await _context.Set<T>()
                .Skip(queryParameters.StartIndex)
                .Take(queryParameters.PageSize)
                .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PagedResult<TResult>
            {
                Items = items,
                PageNumber = queryParameters.PageNumber,
                RecordNumber = queryParameters.PageSize,
                TotalCount = totalSize,
            };
        }

        public async Task<T> GetAsync(int? id)
        {
            _logger.LogInformation($"(Repository) {nameof(GetAsync)}");

            if (id == null)
                return null;

            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            _logger.LogInformation($"(Repository) {nameof(FindBy)}");

            return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _logger.LogInformation($"(Repository) {nameof(UpdateAsync)}");

            _context.Update(entity);

            await _context.SaveChangesAsync();
        }
    }
}
