namespace HotelListingAPI.CustomExceptionMiddleware.Models
{
    public class ErrorDetails
    {
        public int StatusCode{ get; set; }
        public string ErrorMessage { get; set; }
    }
}
