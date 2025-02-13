using CivittaTest.API.Constants;
using CivittaTest.API.Models.Enrico;
using CivittaTest.API.Services.Interfaces;
using RestSharp;

namespace CivittaTest.API.Services.Implementation
{
    public class EnricoService : IEnricoService
    {
        private readonly RestClient _restClient;

        public EnricoService()
        {
            _restClient = new RestClient(AppConstants.ENRICO_API_URL);
        }

        public async Task<List<CountryResponseModel>> GetSupportedCountries() =>
            await SendRequestAsync<List<CountryResponseModel>>("getSupportedCountries") ?? [];

        public async Task<List<HolidayResponseModel>> GetHolidaysForYear(int year, string countryCode, string? region) =>
            await SendRequestAsync<List<HolidayResponseModel>>("getHolidaysForYear", new Dictionary<string, object?>
            {
                        { "year", year },
                        { "country", countryCode },
                        { "region", region }
            }) ?? [];

        public async Task<bool> IsPublicHoliday(DateTime date, string countryCode, string? region) =>
            await SendRequestAsync<PublicHolidayResponseModel>("isPublicHoliday", new Dictionary<string, object?>
            {
                { "date", date.ToString("yyyy-MM-dd") },
                { "country", countryCode },
                { "region", region }
            }) is { IsPublicHoliday: true };

        public async Task<bool> IsWorkDay(DateTime date, string countryCode, string? region) =>
            await SendRequestAsync<WorkDayResponseModel>("isWorkDay", new Dictionary<string, object?>
            {
                { "date", date.ToString("yyyy-MM-dd") },
                { "country", countryCode },
                { "region", region }
            }) is { IsWorkDay: true };

        private async Task<T?> SendRequestAsync<T>(string endpoint, Dictionary<string, object?>? parameters = null)
        {
            var request = new RestRequest(endpoint, Method.Get);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    if (param.Value != null)
                    {
                        request.AddParameter(param.Key, param.Value.ToString());
                    }
                }
            }
            
            return await _restClient.GetAsync<T>(request);
        }
    }
}
