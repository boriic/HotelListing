namespace HotelListingAPI.CustomExceptionMiddleware.CustomExceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name) : base(name)
        {

        }
        public NotFoundException(string name, object key) : base ($"{name} ({key}) was not found")
        {

        }
    }
}
