using HotelListingAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.DAL.Context
{
    public class HotelListingDbContext : DbContext
    {
        public HotelListingDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasData(new Country
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
            }
            );
            modelBuilder.Entity<Hotel>().HasData(new Hotel
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
            }
            );
        }
    }
}
