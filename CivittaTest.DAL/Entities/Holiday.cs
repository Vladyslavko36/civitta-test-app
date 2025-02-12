using System.ComponentModel.DataAnnotations;

namespace CivittaTest.DAL.Entities
{
    public class Holiday
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public int CountryId { get; set; }

        public int? RegionId { get; set; }

        public DateTime Date { get; set; }

        public Region Region { get; set; } = null!;

        public Country Country { get; set; } = null!;
    }
}
