using HotelListingAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListingAPI.DAL.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
            new Country
            {
                Id = 1,
                Name = "Croatia",
                ShortName = "CRO"
            },
            new Country
            {
                Id = 2,
                Name = "Germany",
                ShortName = "GER"
            });
        }
    }
}
