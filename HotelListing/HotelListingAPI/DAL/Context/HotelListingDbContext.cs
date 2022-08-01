using HotelListingAPI.DAL.Configurations;
using HotelListingAPI.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.DAL.Context
{
    public class HotelListingDbContext : IdentityDbContext<User>
    {
        public HotelListingDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new HotelConfiguration());
        }
    }
}
