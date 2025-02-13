namespace CivittaTest.API.Models.HolidayOperations
{
    public class CountryListResponseModel
    {
        public string CountryCode { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<string> Regions { get; set; } = [];
    }
}
