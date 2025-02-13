namespace CivittaTest.API.Models.Enrico
{
    public class CountryResponseModel
    {
        public string CountryCode { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public Date FromDate { get; set; }

        public Date ToDate { get; set; }

        public List<string> Regions { get; set; } = [];
    }
}
