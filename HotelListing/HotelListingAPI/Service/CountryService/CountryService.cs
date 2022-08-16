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

        public async Task AddAsync(CountryCreateDto createCountryDto)
        {
            _logger.LogInformation($"(Service) Trying to create country");

            await _unitOfWork.CountryRepository.AddAsync<CountryCreateDto, Country>(createCountryDto);
            
            await _unitOfWork.Complete();
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation($"(Service) {nameof(DeleteAsync)} ({id})");

            await _unitOfWork.CountryRepository.DeleteAsync(id);

            await _unitOfWork.Complete();
        }
        public async Task<CountryGetUpdateDto> GetAsync(int id)
        {
            _logger.LogInformation($"(Service) {nameof(GetAsync)} ({id})");

            var country = await _unitOfWork.CountryRepository.GetAsync<CountryGetUpdateDto>(id);

            return country;
        }

        public async Task<List<CountryGetUpdateDto>> GetAllAsync()
        {
            _logger.LogInformation($"(Service) {nameof(GetAllAsync)}");

            var countries = await _unitOfWork.CountryRepository.GetAllAsync<CountryGetUpdateDto>();

            return countries;
        }

        public async Task<PagedResult<CountryGetUpdateDto>> GetAllAsync(QueryParameters queryParameters)
        {
            _logger.LogInformation($"(Service) {nameof(GetAllAsync)} Paged");

            var pagedCountries = await _unitOfWork.CountryRepository.GetAllAsync<CountryGetUpdateDto>(queryParameters);

            return pagedCountries;
        }

        public async Task<CountryDto> GetDetailsAsync(int id)
        {
            _logger.LogInformation($"(Service) {nameof(GetDetailsAsync)} ({id})");

            var country = await _unitOfWork.CountryRepository.GetDetailsAsync(id);

            return country;
        }

        public async Task UpdateAsync(int id, CountryGetUpdateDto updateCountryDto)
        {
            _logger.LogInformation($"(Service) {nameof(UpdateAsync)} ({id})");

            await _unitOfWork.CountryRepository.UpdateAsync(id, updateCountryDto);

            await _unitOfWork.Complete();
        }

        public async Task<bool> CountryExists(string countryName)
        {
            _logger.LogInformation($"(Service) {nameof(CountryExists)} ({countryName})");

            var countryFound = await _unitOfWork.CountryRepository.FindBy(x => x.Name == countryName) != null;

            return countryFound;
        }
        public async Task<bool> CountryExists(int countryId)
        {
            _logger.LogInformation($"(Service) {nameof(CountryExists)} ({countryId})");

            var countryFound = await _unitOfWork.CountryRepository.Exists(countryId);

            return countryFound;
        }
    }
}
