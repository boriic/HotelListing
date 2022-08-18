using HotelListingAPI.API.Models;
using HotelListingAPI.API.Models.Hotel;
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

        /// <summary>
        /// Method asynchronously adds new hotel to the database
        /// </summary>
        /// <param name="createHotelDto">Hotel object that will be added to the database</param>
        /// <returns></returns>
        public async Task AddAsync(HotelCreateDto createHotelDto)
        {
            _logger.LogInformation($"(Service) {nameof(AddAsync)}");

            await _unitOfWork.HotelRepository.AddAsync(createHotelDto);

            await _unitOfWork.Complete();
        }

        /// <summary>
        /// Method asynchronously deletes the hotel from the database
        /// </summary>
        /// <param name="id">Id of the hotel that you want to delete</param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation($"(Service) {nameof(DeleteAsync)} ({id})");

            await _unitOfWork.HotelRepository.DeleteAsync(id);

            await _unitOfWork.Complete();
        }

        /// <summary>
        /// Method asynchronously retrieves all the hotels from the database.
        /// </summary>
        /// <returns>List of all the hotels</returns>
        public async Task<List<HotelGetUpdateDto>> GetAllAsync()
        {
            _logger.LogInformation($"(Service) {nameof(GetAllAsync)}");

            var hotels = await _unitOfWork.HotelRepository.GetAllAsync<HotelGetUpdateDto>();

            return hotels;
        }

        /// <summary>
        /// Method asynchronously retrieves all the hotels from the database with query parameters
        /// </summary>
        /// <param name="queryParameters">Parameters you want to apply when retrieving countries.StartIndex,PageSize,PageNumber</param>
        /// <returns>Paged result of all the hotels</returns>
        public async Task<PagedResult<HotelGetUpdateDto>> GetAllAsync(QueryParameters queryParameters)
        {
            _logger.LogInformation($"(Service) {nameof(GetAllAsync)} Paged");

            var pagedHotels = await _unitOfWork.HotelRepository.GetAllAsync<HotelGetUpdateDto>(queryParameters);

            return pagedHotels;
        }

        /// <summary>
        /// Method asynchronously retrieves hotel with the chosen id
        /// </summary>
        /// <param name="id">Id of the hotel to be retrieved</param>
        /// <returns>Hotel</returns>
        public async Task<HotelDto> GetAsync(int id)
        {
            _logger.LogInformation($"(Service) {nameof(GetAsync)} ({id})");

            var country = await _unitOfWork.HotelRepository.GetDetailsAsync(id);

            return country;
        }

        /// <summary>
        /// Method asynchronously updates the hotel.
        /// </summary>
        /// <param name="id">Id of the hotel that you want to update</param>
        /// <param name="updateHotelDto">Hotel object with the updated values</param>
        /// <returns></returns>
        public async Task UpdateAsync(int id, HotelGetUpdateDto updateHotelDto)
        {
            _logger.LogInformation($"(Service) {nameof(UpdateAsync)} ({id})");

            await _unitOfWork.HotelRepository.UpdateAsync(id, updateHotelDto);

            await _unitOfWork.Complete();
        }

        /// <summary>
        /// Method asynchronously checks if a hotel with the provided name exists.
        /// </summary>
        /// <param name="hotelName">Name of the hotel you want to check if exists</param>
        /// <returns>Boolean</returns>
        public async Task<bool> HotelExists(string hotelName)
        {
            _logger.LogInformation($"(Service) {nameof(HotelExists)} ({hotelName})");

            var countryFound = await _unitOfWork.HotelRepository.FindBy<HotelGetUpdateDto>(x => x.Name == hotelName) != null;

            return countryFound;
        }
        /// <summary>
        /// Method asynchronously checks if a hotel with the provided id exists.
        /// </summary>
        /// <param name="hotelId">Id of the hotel you want to check if exists</param>
        /// <returns>Boolean</returns>
        public async Task<bool> HotelExists(int hotelId)
        {
            _logger.LogInformation($"(Service) {nameof(HotelExists)} ({hotelId})");

            var countryFound = await _unitOfWork.CountryRepository.Exists(hotelId);

            return countryFound;
        }

        /// <summary>
        /// Method checks if any other hotels in the database excluding the one that is being updated already has that name
        /// This is used to prevent having duplicate hotels in the database since add method also checks if hotels already exists before inserting
        /// </summary>
        /// <param name="hotelName">Updated name of the country</param>
        /// <param name="HotelId">Id of the country that will get updated</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">This exception will get thrown if there is a hotel already with that name</exception>
        public async Task CheckIfOtherHotelWithNameExists(string hotelName, int hotelId)
        {
            var countries = await GetAllAsync();

            foreach (var item in countries)
            {
                if (item.Id != hotelId)
                {
                    if (item.Name == hotelName)
                        throw new ArgumentException($"Hotel with name: {hotelName}, already exists");
                }
            }
        }
    }
}
