using System.ComponentModel.DataAnnotations;

namespace HotelListingAPI.Auth.Models
{
    public class UserDto : LoginDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
