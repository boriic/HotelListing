﻿using System.ComponentModel.DataAnnotations;

namespace HotelListingAPI.API.Models.Country
{
    public abstract class BaseCountryDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ShortName { get; set; }
    }
}
