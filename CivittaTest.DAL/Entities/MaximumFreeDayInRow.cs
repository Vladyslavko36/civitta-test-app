using System.ComponentModel.DataAnnotations;

namespace CivittaTest.DAL.Entities
{
    public class MaximumFreeDayInRow
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int Year { get; set; }

        [MaxLength(3)]
        public string CountryCode { get; set; } = null!;

        [MaxLength(3)]
        public string? Region { get; set; }

        public int Count { get; set; }
    }
}
