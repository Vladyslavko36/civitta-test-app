using CivittaTest.API.Models.Enrico;

namespace CivittaTest.API.Models.HolidayOperations
{
    public class HolidayModel
    {
        public List<HolidayNameModel> Names { get; set; } = [];

        public DateTime Date { get; set; }
    }
}
