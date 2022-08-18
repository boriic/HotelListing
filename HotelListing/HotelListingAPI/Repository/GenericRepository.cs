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

        /// <summary>
        /// Method asynchronously adds new item to the database.
        /// </summary>
        /// <typeparam name="TSource">Source object that will be passed to the method</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public async Task AddAsync<TSource>(TSource source)
        {
            _logger.LogInformation($"(Repository) {nameof(AddAsync)}");

            var entity = _mapper.Map<T>(source);

            await _context.AddAsync(entity);

            await _context.SaveChangesAsync();

            //TResult can be added which would be the DTO and you can map the entity to that DTO and return it if needed
            //return _mapper.Map<TResult>(entity);
        }

        /// <summary>
        /// Method asynchronously deletes the item from the database.
        /// </summary>
        /// <param name="id">Id of the item you want to delete</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">If item with that id is not found NotFoundException will be thrown which will have entity name
        /// and the id of the item</exception>
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

        /// <summary>
        /// Method asynchronously checks if item exists in the database.
        /// </summary>
        /// <param name="id">Id of the item you want to check if exists</param>
        /// <returns>Boolean</returns>
        public async Task<bool> Exists(int id)
        {
            _logger.LogInformation($"(Repository) {nameof(Exists)} ({id})");

            var entity = await GetAsync(id);

            return entity != null;
        }

        /// <summary>
        /// Method asynchronously gets all the items from the database.
        /// </summary>
        /// <typeparam name="TResult">Result object that would entity object get mapped to and this method would return it, for example: CountryDTO and etc.</typeparam>
        /// <returns>List of all the items from the database with the TResult type</returns>
        public async Task<List<TResult>> GetAllAsync<TResult>()
        {
            _logger.LogInformation($"(Repository) {nameof(GetAllAsync)}");

            return await _context.Set<T>().ProjectTo<TResult>(_mapper.ConfigurationProvider).ToListAsync();
        }

        /// <summary>
        /// Method asynchronously retrieves all the items from the database with query parameters
        /// </summary>
        /// <typeparam name="TResult">Result object that would entity object get mapped to and this method would return it, for example: CountryDTO and etc.</typeparam>
        /// <param name="queryParameters">Parameters you want to apply when retrieving items.StartIndex,PageSize,PageNumber</param>
        /// <returns>Paged result of all the items with the TResult type</returns>
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
                PageSize = queryParameters.PageSize,
                TotalCount = totalSize,
            };
        }

        /// <summary>
        /// Method asynchronously retrieves the item from the database
        /// </summary>
        /// <typeparam name="TResult">Result object that would entity object get mapped to and this method would return it, for example: CountryDTO and etc.</typeparam>
        /// <param name="id">Id of the item you want to find</param>
        /// <returns>Item with the TResult type</returns>
        /// <exception cref="NotFoundException">If item with that id is not found NotFoundException will be thrown which will have entity name
        /// and the id of the item</exception>        
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
        /// <summary>
        /// Method asynchronously retrieves the item from the database with the provided expression
        /// </summary>
        /// <param name="predicate">Expression you want to use when finding item, for example: CountryRepository.FindBy(x => x.Name == countryName)</param>
        /// <returns>Item with the TResult type</returns>
        public async Task<TResult> FindBy<TResult>(Expression<Func<T, bool>> predicate)
        {
            _logger.LogInformation($"(Repository) {nameof(FindBy)}");

            return await _context.Set<T>().Where(predicate).ProjectTo<TResult>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Method asynchronously updates the item in the database
        /// </summary>
        /// <typeparam name="TSource">Source type of the object that will be passed</typeparam>
        /// <param name="id">Id of the item that will be updated</param>
        /// <param name="source">Source object that has the updated values</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">If item with that id is not found NotFoundException will be thrown which will have entity name
        /// and the id of the item</exception>   
        public async Task UpdateAsync<TSource>(int id, TSource source)
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

        /// <summary>
        /// Private method that asynchronously retrieves the item from the database which is used for public methods inside repository
        /// to make it look cleaner when finding the item before doing something with them.
        /// </summary>
        /// <param name="id">Id of the item that you want to find</param>
        /// <returns>Item that was found</returns>
        /// <exception cref="NotFoundException">If item with that id is not found NotFoundException will be thrown which will have entity name
        /// and the id of the item</exception>   
        private async Task<T> GetAsync(int? id)
        {
            _logger.LogInformation($"(Repository) {nameof(GetAsync)}");

            if (id == null)
                throw new NotFoundException(typeof(T).Name, id.HasValue ? id : "No Key Provided");

            var entity = await _context.Set<T>().FindAsync(id);

            if (entity == null)
                throw new NotFoundException(typeof(T).Name, id);

            return entity;
        }
    }
}
