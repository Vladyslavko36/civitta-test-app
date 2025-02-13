using System.ComponentModel.DataAnnotations;

namespace CivittaTest.DAL.Entities
{
    public class HolidayName
    {
        public int Id { get; set; }

        [MaxLength(2)]
        public string Lang { get; set; } = null!;

        [MaxLength(100)]
        public string Text { get; set; } = null!;

        public int HolidayId { get; set; }

        public virtual Holiday Holiday { get; set; }
    }
}
