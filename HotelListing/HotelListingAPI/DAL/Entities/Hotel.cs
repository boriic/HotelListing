using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListingAPI.DAL.Entities
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public double Rating { get; set; }
        public int Capacity { get; set; }
        [ForeignKey(nameof(CountryId))]
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
