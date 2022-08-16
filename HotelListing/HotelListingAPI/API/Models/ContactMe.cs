using System.ComponentModel.DataAnnotations;

namespace HotelListingAPI.API.Models
{
    public class ContactMe
    {
        [Required]
        public string SenderName { get; set; }
        [Required, EmailAddress]
        public string SenderEmail { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
