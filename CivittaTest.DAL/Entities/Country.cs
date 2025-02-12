using System.ComponentModel.DataAnnotations;

namespace CivittaTest.DAL.Entities
{
    public class Country
    {
        public int Id { get; set; }

        [MaxLength(3)]
        public string CountryCode { get; set; } = null!;

        [MaxLength(150)]
        public string FullName { get; set; } = null!;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public virtual ICollection<Region> Regions { get; set; } = null!;
    }
}
