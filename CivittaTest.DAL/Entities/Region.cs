using System.ComponentModel.DataAnnotations;

namespace CivittaTest.DAL.Entities
{
    public class Region
    {
        public int Id { get; set; }

        [MaxLength(3)]
        public string Code { get; set; } = null!;

        public int CountryId { get; set; }

        public virtual Country Country { get; set; } = null!;
    }
}
