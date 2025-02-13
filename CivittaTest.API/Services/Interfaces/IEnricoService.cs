using CivittaTest.API.Models.Enrico;

namespace CivittaTest.API.Services.Interfaces
{
    public interface IEnricoService
    {
        Task<List<CountryResponseModel>> GetSupportedCountries();

        Task<List<HolidayResponseModel>> GetHolidaysForYear(int year, string countryCode, string? region);

        Task<bool> IsPublicHoliday(DateTime date, string countryCode, string? region);

        Task<bool> IsWorkDay(DateTime date, string countryCode, string? region);
    }
}
