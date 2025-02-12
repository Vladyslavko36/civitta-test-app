using System.ComponentModel.DataAnnotations;

namespace CivittaTest.DAL.Entities
{
    public class DayStatus
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        [MaxLength(3)]
        public string CountryCode { get; set; } = null!;

        [MaxLength(3)]
        public string? CountryRegion { get; set; }

        public bool IsWorkDay { get; set; }

        public bool IsHoliday { get; set; }
    }
}
