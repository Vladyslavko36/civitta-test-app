using CivittaTest.API.Services.Implementation;
using CivittaTest.API.Services.Interfaces;
using CivittaTest.DAL.Context;
using CivittaTest.DAL.Entities;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;

public class HolidayOperationsServiceTests
{
    private readonly DbContextOptions<AppDbContext> _dbOptions;

    public HolidayOperationsServiceTests()
    {
        _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;
    }

    [Fact]
    public async Task GetCountriesAsync_ShouldReturnCountries_WhenDataExists()
    {
        // Arrange
        using var context = new AppDbContext(_dbOptions);
        context.Countries.Add(new Country
        {
            CountryCode = "UA",
            FullName = "Ukraine",
            FromDate = DateTime.UtcNow.AddYears(-10),
            ToDate = DateTime.UtcNow.AddYears(10),
            Regions = new List<Region> { new Region { Code = "lvl" } }
        });
        await context.SaveChangesAsync();

        var service = new HolidayOperationsService(context, A.Fake<IEnricoService>());

        // Act
        var result = await service.GetCountriesAsync();

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal("UA", result.First().CountryCode);
        Assert.Contains("lvl", result.First().Regions);
    }

    [Fact]
    public async Task GetGroupedByMonthHolidaysAsync_ShouldReturnHolidays_WhenDataExists()
    {
        // Arrange
        using var context = new AppDbContext(_dbOptions);
        var country = new Country { CountryCode = "ukr", FullName = "Ukraine" };
        context.Countries.Add(country);
        context.Holidays.Add(new Holiday
        {
            Country = country,
            Date = new DateTime(2024, 1, 1),
            HolidayNames = new List<HolidayName>
            {
                new HolidayName { Lang = "en", Text = "New Year" }
            }
        });
        await context.SaveChangesAsync();

        var service = new HolidayOperationsService(context, A.Fake<IEnricoService>());

        // Act
        var result = await service.GetGroupedByMonthHolidaysAsync(2024, "UA");

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, g => g.Month == 1);
        Assert.Contains(result.First().Holidays, h => h.Names.Any(n => n.Text == "New Year"));
    }

    [Fact]
    public async Task GetDayStatusAsync_ShouldReturnCorrectStatus()
    {
        // Arrange
        using var context = new AppDbContext(_dbOptions);
        context.DayStatuses.Add(new DayStatus
        {
            CountryCode = "ukr",
            Date = new DateTime(2024, 2, 23),
            IsWorkDay = false,
            IsHoliday = true
        });
        await context.SaveChangesAsync();

        var service = new HolidayOperationsService(context, A.Fake<IEnricoService>());

        // Act
        var result = await service.GetDayStatusAsync(new DateTime(2024, 2, 23), "ukr");

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsWorkDay);
        Assert.True(result.IsHoliday);
        Assert.True(result.IsFreeDay);
    }

    [Fact]
    public async Task GetMaxFreeDaysInRowAsync_ShouldReturnCorrectMaxStreak()
    {
        // Arrange
        using var context = new AppDbContext(_dbOptions);
        context.MaximumFreeDayInRows.Add(new MaximumFreeDayInRow
        {
            Year = 2024,
            CountryCode = "ukr",
            Count = 5
        });
        await context.SaveChangesAsync();

        var service = new HolidayOperationsService(context, A.Fake<IEnricoService>());

        // Act
        var result = await service.GetMaxFreeDaysInRowAsync(2024, "ukr");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Count);
    }
}
