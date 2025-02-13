namespace CivittaTest.API.Models.Enrico
{
    public struct Date
    {
        public int Day { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int DayOfWeek { get; set; }

        public DateTime ToDateTime() => new(Year > 9999 ? 9999 : Year, Month, Day);
    }
}
