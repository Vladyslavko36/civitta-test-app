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
        /// <summary>
        /// Retrieves a list of countries with their details.
        /// </summary>
        /// <returns>A list of country response models.</returns>
        /// <response code="200">Returns the list of countries successfully.</response>
        /// <response code="400">Bad request if the data is invalid or missing.</response>
        [HttpGet("getCountries")]
        public async Task<ActionResult<List<CountryListResponseModel>>> GetCountries()
        {
            var response = await service.GetCountriesAsync();

            return Ok(response);
        }

        /// <summary>
        /// Retrieves holidays grouped by month for a given year and country code.
        /// </summary>
        /// <param name="year">The year for which holidays are retrieved.</param>
        /// <param name="countryCode">The country code for which holidays are retrieved.</param>
        /// <returns>A list of grouped holidays for the specified country and year.</returns>
        /// <remarks>
        /// Both 'year' and 'countryCode' are required. If any is missing, the request will return a BadRequest.
        /// </remarks>
        /// <response code="200">Returns the grouped holidays for the specified country and year.</response>
        /// <response code="400">Bad request if either 'year' or 'countryCode' is missing or invalid.</response>
        [HttpGet("getGroupedHolidays")]
        public async Task<ActionResult<List<GroupedHolidaysModel>>> GetGroupedByMonthHolidays(
            [FromQuery][Required] int year,
            [FromQuery][Required] string? countryCode)
        {
            if (year == default || countryCode is null)
            {
                return BadRequest("Both 'year' and 'countryCode' are required.");
            }

            var response = await service.GetGroupedByMonthHolidaysAsync(year, countryCode);

            return Ok(response);
        }

        /// <summary>
        /// Retrieves the day status (workday, free day, holiday) for a given date and country code.
        /// </summary>
        /// <param name="date">The date to check the status for.</param>
        /// <param name="countryCode">The country code to check the status for.</param>
        /// <returns>The day status for the given date and country code.</returns>
        /// <remarks>
        /// Both 'date' and 'countryCode' are required. If any is missing, the request will return a BadRequest.
        /// </remarks>
        /// <response code="200">Returns the day status (workday, free day, holiday) for the given date and country code.</response>
        /// <response code="400">Bad request if either 'date' or 'countryCode' is missing or invalid.</response>
        [HttpGet("getDayStatus")]
        public async Task<IActionResult> GetDayStatus(
            [FromQuery][Required] DateTime date,
            [FromQuery][Required] string countryCode)
        {
            if (date == default || countryCode == null)
            {
                return BadRequest("Both 'year' and 'countryCode' are required.");
            }

            var response = await service.GetDayStatusAsync(date, countryCode);

            return Ok(response);
        }

        /// <summary>
        /// Retrieves the maximum number of free (holiday + free) days in a row for a given year and country code.
        /// </summary>
        /// <param name="year">The year to check for maximum consecutive free days.</param>
        /// <param name="countryCode">The country code to check the free days for.</param>
        /// <returns>The maximum number of consecutive free days for the given year and country code.</returns>
        /// <remarks>
        /// Both 'year' and 'countryCode' are required. If any is missing, the request will return a BadRequest.
        /// </remarks>
        /// <response code="200">Returns the maximum number of free days in a row for the given year and country code.</response>
        /// <response code="400">Bad request if either 'year' or 'countryCode' is missing or invalid.</response>
        [HttpGet("getMaxFreeDaysInRow")]
        public async Task<IActionResult> GetMaxFreeDaysInRow(
            [FromQuery][Required] int year,
            [FromQuery][Required] string countryCode)
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
