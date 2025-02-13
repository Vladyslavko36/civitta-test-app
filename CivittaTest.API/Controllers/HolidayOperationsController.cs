using System.ComponentModel.DataAnnotations;
using CivittaTest.API.Models.HolidayOperations;
using CivittaTest.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CivittaTest.API.Controllers
{
    [Route("api")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class HolidayOperationsController(IHolidayOperationsService service) : ControllerBase
    {
        [HttpGet("getCountries")]
        public async Task<ActionResult<List<CountryListResponseModel>>> GetCountries()
        {
            var countries = await service.GetCountriesAsync();
            return Ok(countries);
        }

        [HttpGet("getGroupedHolidays")]
        public async Task<ActionResult<List<GroupedHolidaysModel>>> GetGroupedByMonthHolidays(
            [FromQuery][Required] int year,
            [FromQuery][Required] string? countryCode)
        {
            if (year == default || countryCode is null)
            {
                return BadRequest("Both 'year' and 'countryCode' are required.");
            }

            var holidays = await service.GetGroupedByMonthHolidaysAsync(year, countryCode);
            return Ok(holidays);
        }

        [HttpGet("day-status")]
        public async Task<IActionResult> GetDayStatus([FromQuery][Required] DateTime date, [FromQuery][Required] string countryCode)
        {
            if (date == default || countryCode == null)
            {
                return BadRequest("Both 'year' and 'countryCode' are required.");
            }

            var response = await service.GetDayStatusAsync(date, countryCode);

            return Ok(response);
        }

        [HttpGet("max-free-days")]
        public async Task<IActionResult> GetMaxFreeDaysInRow([FromQuery, Required] int year, [FromQuery, Required] string countryCode)
        {
            if (year == default || countryCode == null)
            {
                return BadRequest("Both 'year' and 'countryCode' are required.");
            }

            var response = await service.GetMaxFreeDaysInRowAsync(year, countryCode);

            return Ok(response);
        }
    }
}
