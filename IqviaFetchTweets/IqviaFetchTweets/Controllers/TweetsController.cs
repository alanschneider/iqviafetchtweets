using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IqviaFetchTweets.Exceptions;
using IqviaFetchTweets.Interfaces;
using IqviaFetchTweets.OldApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IqviaFetchTweets.Controllers
{
    [Produces("application/json")]
    [Route("api/1/tweets")]
    public class TweetsController : Controller
    {
        private readonly ITweetsSettings _settings;
        private readonly IOldApi _oldApi;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settings">Instance of ITweetsSettings.</param>
        /// <param name="oldApi">Instance of IOldApi.</param>
        public TweetsController(ITweetsSettings settings, IOldApi oldApi)
        {
            _settings = settings;
            _oldApi = oldApi;
        }

        /// <summary>
        /// Validates that a date/time object is valid for this controller.
        /// Throws a BadRequestException if the validation fails.
        /// </summary>
        /// <param name="dt">DateTime instance</param>
        /// <param name="dtName">Name of DateTime instance.</param>
        private void ThrowIfNotInValidDateRange(DateTime dt, string dtName)
        {
            var minDate = DateTime.UtcNow.AddYears(-_settings.MaxYearsIntoPast);

            if (dt <= DateTime.MinValue)
                throw new BadRequestException($"{dtName} must be specified.");
            if (dt > DateTime.UtcNow)
                throw new BadRequestException($"{dtName} cannot be in the future.");

            if (dt < minDate)
                throw new BadRequestException($"Cannot retrieve records before {minDate}. Specify a later date/time for {dtName}.");
        }

        /// <summary>
        /// Returns all unique tweets from a given date range. 
        /// </summary>
        /// <param name="startDate">Starting date range. The number of years 
        /// back is capped in the config file, so a startDate that precedes 
        /// this will throw an exception.
        /// </param>
        /// <param name="endDate">Ending date range. Must be less than the current date/time.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<string>> Get([FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            ThrowIfNotInValidDateRange(startDate, "startDate");
            ThrowIfNotInValidDateRange(endDate, "endDate");
            if (startDate >= endDate)
                throw new BadRequestException("endDate must be greater than startDate.");

            return await _oldApi.GetUniqueTweets(startDate, endDate);
        }
    }
}