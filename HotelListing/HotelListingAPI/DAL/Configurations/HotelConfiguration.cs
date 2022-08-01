using HotelListingAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListingAPI.DAL.Configurations
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
           new Hotel
           {
               Id = 1,
               Name = "Family Hotel Diadora",
               Address = "Test",
               Rating = 5.0,
               CountryId = 1
           },
           new Hotel
           {
               Id = 2,
               Name = "Germany Test Hotel",
               Address = "Test Germany",
               Rating = 4.4,
               CountryId = 2
           });
        }
    }
}
