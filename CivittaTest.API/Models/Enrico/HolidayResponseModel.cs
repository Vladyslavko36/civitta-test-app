namespace CivittaTest.API.Models.Enrico
{
    public class HolidayResponseModel
    {
        public Date Date { get; set; }

        public List<HolidayNameModel> Name { get; set; } = null!;    
    }
}
