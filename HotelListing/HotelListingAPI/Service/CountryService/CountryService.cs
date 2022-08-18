using HotelListingAPI.API.Models;
using HotelListingAPI.API.Models.Country;
using HotelListingAPI.DAL.Entities;
using HotelListingAPI.Repository.Common;
using HotelListingAPI.Service.Common.CountryService;

namespace HotelListingAPI.Service.CountryService
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryService> _logger;

        public CountryService(IUnitOfWork unitOfWork, ILogger<CountryService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Method asynchronously adds new country to the database
        /// </summary>
        /// <param name="createCountryDto">Country object that will be added to the database</param>
        /// <returns></returns>
        public async Task AddAsync(CountryCreateDto createCountryDto)
        {
            _logger.LogInformation($"(Service) {nameof(AddAsync)}");

            await _unitOfWork.CountryRepository.AddAsync(createCountryDto);
            
            await _unitOfWork.Complete();
        }

        /// <summary>
        /// Method asynchronously deletes the country from the database
        /// </summary>
        /// <param name="id">Id of the country that you want to delete</param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation($"(Service) {nameof(DeleteAsync)} ({id})");

            await _unitOfWork.CountryRepository.DeleteAsync(id);

            await _unitOfWork.Complete();
        }

        /// <summary>
        /// Method asynchronously retrieves all the countries from the database.
        /// </summary>
        /// <returns>List of all the countries</returns>
        public async Task<List<CountryGetUpdateDto>> GetAllAsync()
        {
            _logger.LogInformation($"(Service) {nameof(GetAllAsync)}");

            var countries = await _unitOfWork.CountryRepository.GetAllAsync<CountryGetUpdateDto>();

            return countries;
        }

        /// <summary>
        /// Method asynchronously retrieves all the countries from the database with query parameters
        /// </summary>
        /// <param name="queryParameters">Parameters you want to apply when retrieving countries.StartIndex,PageSize,PageNumber</param>
        /// <returns>Paged result of all the countries</returns>
        public async Task<PagedResult<CountryGetUpdateDto>> GetAllAsync(QueryParameters queryParameters)
        {
            _logger.LogInformation($"(Service) {nameof(GetAllAsync)} Paged");

            var pagedCountries = await _unitOfWork.CountryRepository.GetAllAsync<CountryGetUpdateDto>(queryParameters);

            return pagedCountries;
        }

        /// <summary>
        /// Method asynchronously retrieves country with the chosen id
        /// </summary>
        /// <param name="id">Id of the country to be retrieved</param>
        /// <returns>Country</returns>
        public async Task<CountryDto> GetAsync(int id)
        {
            _logger.LogInformation($"(Service) {nameof(GetAsync)} ({id})");

            var country = await _unitOfWork.CountryRepository.GetDetailsAsync(id);

            return country;
        }

        /// <summary>
        /// Method asynchronously updates the country.
        /// </summary>
        /// <param name="id">Id of the country that you want to update</param>
        /// <param name="updateCountryDto">Country object with the updated values</param>
        /// <returns></returns>
        public async Task UpdateAsync(int id, CountryGetUpdateDto updateCountryDto)
        {
            _logger.LogInformation($"(Service) {nameof(UpdateAsync)} ({id})");

            await _unitOfWork.CountryRepository.UpdateAsync(id, updateCountryDto);

            await _unitOfWork.Complete();
        }
        /// <summary>
        /// Method asynchronously checks if a country with the provided name exists.
        /// </summary>
        /// <param name="countryName">Name of the country you want to check if exists</param>
        /// <returns>Boolean</returns>
        public async Task<bool> CountryExists(string countryName)
        {
            _logger.LogInformation($"(Service) {nameof(CountryExists)} ({countryName})");

            var countryFound = await _unitOfWork.CountryRepository.FindBy<CountryGetUpdateDto>(x => x.Name == countryName) != null;

            return countryFound;
        }
        /// <summary>
        /// Method asynchronously checks if a country with the provided id exists.
        /// </summary>
        /// <param name="countryId">Id fo the country you want to check if exists</param>
        /// <returns>Boolean</returns>
        public async Task<bool> CountryExists(int countryId)
        {
            _logger.LogInformation($"(Service) {nameof(CountryExists)} ({countryId})");

            var countryFound = await _unitOfWork.CountryRepository.Exists(countryId);

            return countryFound;
        }

        /// <summary>
        /// Method checks if any other country in the database excluding the on that is being updated already has that name
        /// This is used to prevent having duplicate countries in the database since add method also checks if country already exists before inserting
        /// </summary>
        /// <param name="countryName">Updated name of the country</param>
        /// <param name="countryId">Id of the country that will get updated</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">This exception will get thrown if there is a country already with that name</exception>
        public async Task CheckIfOtherCountryWithNameExists(string countryName, int countryId)
        {
            var countries = await GetAllAsync();

            foreach (var item in countries)
            {
                if (item.Id != countryId)
                {
                    if (item.Name == countryName)
                        throw new ArgumentException($"Country with name: {countryName}, already exists");
                }
            }
        }
    }
}
