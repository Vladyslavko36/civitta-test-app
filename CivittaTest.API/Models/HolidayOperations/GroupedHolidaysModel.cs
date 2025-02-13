namespace CivittaTest.API.Models.HolidayOperations
{
    public class GroupedHolidaysModel
    {
        public int Month { get; set; }

        public List<HolidayModel> Holidays { get; set; } = [];
    }
}
