using CivittaTest.API.Models.Enrico;
using CivittaTest.API.Models.HolidayOperations;
using CivittaTest.API.Services.Interfaces;
using CivittaTest.DAL.Context;
using CivittaTest.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CivittaTest.API.Services.Implementation
{
    public class HolidayOperationsService(AppDbContext db, IEnricoService enricoService) : IHolidayOperationsService
    {
        private readonly AppDbContext _db = db;
        private readonly IEnricoService _enricoService = enricoService;

        public async Task<List<CountryListResponseModel>> GetCountriesAsync()
        {
            if (!await _db.Countries.AnyAsync())
            {
                await PopulateCountriesData();
            }

            return await _db.Countries
                .Include(x => x.Regions)
                .Select(country => new CountryListResponseModel
                {
                    CountryCode = country.CountryCode,
                    FullName = country.FullName,
                    FromDate = country.FromDate,
                    ToDate = country.ToDate,
                    Regions = country.Regions.Select(s => s.Code).ToList()
                }).ToListAsync();
        }

        public async Task<List<GroupedHolidaysModel>> GetGroupedByMonthHolidaysAsync(int year, string countryCode)
        {
            if (!await _db.Holidays.Include(h => h.Country).AnyAsync(h => h.Date.Year == year && h.Country.CountryCode == countryCode))
            {
                await PopulateHolidaysData(year, countryCode);
            }

            return await _db.Holidays
                .Include(h => h.HolidayNames)
                .Where(h => h.Date.Year == year)
                .GroupBy(h => h.Date.Month)
                .Select(g => new GroupedHolidaysModel
                {
                    Month = g.Key,
                    Holidays = g.Select(x => new HolidayModel
                    {
                        Date = x.Date,
                        Names = x.HolidayNames.Select(x => new HolidayNameModel { Lang = x.Lang, Text = x.Text }).ToList()
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<DayStatusResponseModel> GetDayStatusAsync(DateTime date, string countryCode, string? region = null)
        {
            var dayStatus = await _db.DayStatuses
                .FirstOrDefaultAsync(s =>
                    s.CountryCode == countryCode && s.Date.Date == date.Date)
                ?? await PopulateDayStatus(date, countryCode, region);

            return new DayStatusResponseModel
            {
                IsWorkDay = dayStatus.IsWorkDay,
                IsHoliday = dayStatus.IsHoliday,
                IsFreeDay = !dayStatus.IsWorkDay || dayStatus.IsHoliday
            };
        }

        public async Task<MaxFreeDayInRowResponseModel> GetMaxFreeDaysInRowAsync(int year, string countryCode)
        {
            var existingRecord = await _db.MaximumFreeDayInRows
               .FirstOrDefaultAsync(x => x.Year == year && x.CountryCode == countryCode);

            if (existingRecord == null)
            {
                existingRecord = await PopulateMaxFreeDaysCount(db, year, countryCode);
            }

            return new MaxFreeDayInRowResponseModel { Count = existingRecord.Count };
        }

        #region Private methods

        private static int GetMaxConsecutiveDays(int year, List<DateTime> holidays)
        {
            var start = new DateTime(year, 1, 1);
            var end = new DateTime(year, 12, 31);
            int max = 0, current = 0;

            for (var date = start; date <= end; date = date.AddDays(1))
            {
                var isFree = holidays.Contains(date) || date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;

                current = isFree ? current + 1 : 0;
                max = Math.Max(max, current);
            }

            return max;
        }

        private async Task PopulateCountriesData()
        {
            var countries = await _enricoService.GetSupportedCountries();

            var countriesToAdd = countries.Select(c => new Country
            {
                CountryCode = c.CountryCode,
                FullName = c.FullName,
                FromDate = c.FromDate.ToDateTime(),
                ToDate = c.ToDate.ToDateTime(),
                Regions = c.Regions.Select(r => new Region { Code = r }).ToList()
            });

            await _db.Countries.AddRangeAsync(countriesToAdd);
            await _db.SaveChangesAsync();
        }

        private async Task PopulateHolidaysData(int year, string countryCode, string? region = null)
        {
            if (!await _db.Countries.AnyAsync())
            {
                await PopulateCountriesData();
            }

            var country = await _db.Countries.Include(c => c.Regions)
                .FirstOrDefaultAsync(c => c.CountryCode == countryCode)
                ?? throw new InvalidDataException($"Country code {countryCode} is not exists");

            var holidays = await _enricoService.GetHolidaysForYear(year, countryCode, region);

            var holidaysToAdd = holidays.Select(h => new Holiday
            {
                CountryId = country.Id,
                Date = h.Date.ToDateTime(),
                HolidayNames = h.Name.Select(hn => new HolidayName { Text = hn.Text, Lang = hn.Lang }).ToList(),
                RegionId = !string.IsNullOrEmpty(region) ? country.Regions.FirstOrDefault(r => r.Code == region)?.Id : null
            });

            await _db.Holidays.AddRangeAsync(holidaysToAdd);
            await _db.SaveChangesAsync();
        }

        private async Task<DayStatus> PopulateDayStatus(DateTime date, string countryCode, string? region)
        {
            // TODO: Make two parallel requests
            var isHoliday = await _enricoService.IsPublicHoliday(date.Date, countryCode, region);
            var isWorkday = await _enricoService.IsWorkDay(date.Date, countryCode, region);

            var newDayStatus = new DayStatus
            {
                CountryCode = countryCode,
                Date = date.Date,
                CountryRegion = region,
                IsWorkDay = isWorkday,
                IsHoliday = isHoliday
            };

            await _db.DayStatuses.AddAsync(newDayStatus);
            await _db.SaveChangesAsync();

            return newDayStatus;
        }

        private async Task<MaximumFreeDayInRow> PopulateMaxFreeDaysCount(AppDbContext db, int year, string countryCode)
        {
            if (!await _db.Holidays.Include(h => h.Country).AnyAsync(h => h.Date.Year == year && h.Country.CountryCode == countryCode))
            {
                await PopulateHolidaysData(year, countryCode);
            }

            var holidays = await db.Holidays
                .Include(x => x.Country)
                .Where(h => h.Country.CountryCode == countryCode && h.Date.Year == year)
                .Select(h => h.Date)
                .ToListAsync();

            int max = GetMaxConsecutiveDays(year, holidays);

            var newRecord = new MaximumFreeDayInRow
            {
                Year = year,
                CountryCode = countryCode,
                Count = max
            };

            await _db.MaximumFreeDayInRows.AddAsync(newRecord);
            await _db.SaveChangesAsync();

            return newRecord;
        }

        #endregion
    }
}
