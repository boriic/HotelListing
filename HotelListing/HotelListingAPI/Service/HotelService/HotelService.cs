using HotelListingAPI.API.Models;
using HotelListingAPI.API.Models.Hotel;
using HotelListingAPI.DAL.Entities;
using HotelListingAPI.Repository.Common;
using HotelListingAPI.Service.Common.HotelService;

namespace HotelListingAPI.Service.HotelService
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HotelService> _logger;

        public HotelService(IUnitOfWork unitOfWork, ILogger<HotelService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task AddAsync(HotelCreateDto createHotelDto)
        {
            _logger.LogInformation($"(Service) {nameof(AddAsync)}");

            await _unitOfWork.HotelRepository.AddAsync<HotelCreateDto, Hotel>(createHotelDto);

            await _unitOfWork.Complete();
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation($"(Service) {nameof(DeleteAsync)} ({id})");

            await _unitOfWork.HotelRepository.DeleteAsync(id);

            await _unitOfWork.Complete();
        }
        public async Task<HotelGetUpdateDto> GetAsync(int id)
        {
            _logger.LogInformation($"(Service) {nameof(GetAsync)} ({id})");

            var hotel = await _unitOfWork.HotelRepository.GetAsync<HotelGetUpdateDto>(id);

            return hotel;
        }

        public async Task<List<HotelGetUpdateDto>> GetAllAsync()
        {
            _logger.LogInformation($"(Service) {nameof(GetAllAsync)}");

            var hotels = await _unitOfWork.HotelRepository.GetAllAsync<HotelGetUpdateDto>();

            return hotels;
        }

        public async Task<PagedResult<HotelGetUpdateDto>> GetAllAsync(QueryParameters queryParameters)
        {
            _logger.LogInformation($"(Service) {nameof(GetAllAsync)} Paged");

            var pagedHotels = await _unitOfWork.HotelRepository.GetAllAsync<HotelGetUpdateDto>(queryParameters);

            return pagedHotels;
        }

        public async Task<HotelDto> GetDetailsAsync(int id)
        {
            _logger.LogInformation($"(Service) {nameof(GetDetailsAsync)} ({id})");

            var country = await _unitOfWork.HotelRepository.GetDetailsAsync(id);

            return country;
        }

        public async Task UpdateAsync(int id, HotelGetUpdateDto updateHotelDto)
        {
            _logger.LogInformation($"(Service) {nameof(UpdateAsync)} ({id})");

            await _unitOfWork.HotelRepository.UpdateAsync(id, updateHotelDto);

            await _unitOfWork.Complete();
        }

        public async Task<bool> HotelExists(string hotelName)
        {
            _logger.LogInformation($"(Service) {nameof(HotelExists)} ({hotelName})");

            var countryFound = await _unitOfWork.HotelRepository.FindBy(x => x.Name == hotelName) != null;

            return countryFound;
        }
        public async Task<bool> HotelExists(int hotelId)
        {
            _logger.LogInformation($"(Service) {nameof(HotelExists)} ({hotelId})");

            var countryFound = await _unitOfWork.CountryRepository.Exists(hotelId);

            return countryFound;
        }
    }
}
