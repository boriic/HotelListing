using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListingAPI.API.Models;
using HotelListingAPI.CustomExceptionMiddleware.CustomExceptions;
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
        public async Task AddAsync<TSource, TResult>(TSource source)
        {
            _logger.LogInformation($"(Repository) {nameof(AddAsync)}");

            var entity = _mapper.Map<T>(source);

            await _context.AddAsync(entity);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation($"(Repository) {nameof(DeleteAsync)}");


            var entity = await GetAsync(id);

            if (entity == null)
            {
                throw new NotFoundException(typeof(T).Name, id);
            }

            _context.Set<T>().Remove(entity);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            _logger.LogInformation($"(Repository) {nameof(Exists)} ({id})");

            var entity = await GetAsync(id);

            return entity != null;
        }

        public async Task<List<TResult>> GetAllAsync<TResult>()
        {
            _logger.LogInformation($"(Repository) {nameof(GetAllAsync)}");

            return await _context.Set<T>().ProjectTo<TResult>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)
        {
            _logger.LogInformation($"(Repository) {nameof(GetAllAsync)} Paged");

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

        public async Task<TResult> GetAsync<TResult>(int? id)
        {
            _logger.LogInformation($"(Repository) {nameof(GetAsync)}");

            if (id == null)
                throw new NotFoundException(typeof(T).Name, id.HasValue ? id : "No Key Provided");

            var entity = await _context.Set<T>().FindAsync(id);

            if (entity == null)
                throw new NotFoundException(typeof(T).Name, id);

            return _mapper.Map<TResult>(entity);
        }

        public async Task<T> GetAsync(int? id)
        {
            _logger.LogInformation($"(Repository) {nameof(GetAsync)}");

            if (id == null)
                throw new NotFoundException(typeof(T).Name, id.HasValue ? id : "No Key Provided");

            var entity = await _context.Set<T>().FindAsync(id);

            if (entity == null)
                throw new NotFoundException(typeof(T).Name, id);

            return entity;
        }

        public async Task<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            _logger.LogInformation($"(Repository) {nameof(FindBy)}");

            return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync<TSource>(int id ,TSource source)
        {
            _logger.LogInformation($"(Repository) {nameof(UpdateAsync)}");

            var entity = await GetAsync(id);

            if (entity == null)
            {
                throw new NotFoundException(typeof(T).Name, id);
            }

            _mapper.Map(source, entity);
            _context.Update(entity);

            await _context.SaveChangesAsync();
        }
    }
}
