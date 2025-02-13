namespace CivittaTest.DAL.Entities
{
    public class Holiday
    {
        public int Id { get; set; }

        public int CountryId { get; set; }

        public int? RegionId { get; set; }

        public DateTime Date { get; set; }

        public virtual Region Region { get; set; } = null!;

        public virtual Country Country { get; set; } = null!;

        public virtual ICollection<HolidayName> HolidayNames { get; set; } = null!;
    }
}
