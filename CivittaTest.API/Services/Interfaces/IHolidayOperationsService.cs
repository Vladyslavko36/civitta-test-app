using CivittaTest.API.Models.HolidayOperations;

namespace CivittaTest.API.Services.Interfaces
{
    public interface IHolidayOperationsService
    {
        Task<List<CountryListResponseModel>> GetCountriesAsync();

        Task<DayStatusResponseModel> GetDayStatusAsync(DateTime date, string countryCode, string? region = null);

        Task<List<GroupedHolidaysModel>> GetGroupedByMonthHolidaysAsync(int year, string countryCode);

        Task<MaxFreeDayInRowResponseModel> GetMaxFreeDaysInRowAsync(int year, string countryCode);
    }
}
