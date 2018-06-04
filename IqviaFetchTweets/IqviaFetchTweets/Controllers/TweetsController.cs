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

        public TweetsController(ITweetsSettings settings, IOldApi oldApi)
        {
            _settings = settings;
            _oldApi = oldApi;
        }

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

        //[HttpGet]
        //public IEnumerable<string> Get([FromQuery] DateTime startDate,
        //                               [FromQuery] DateTime endDate)
        //{
        //    ThrowIfNotInValidDateRange(startDate, "startDate");
        //    ThrowIfNotInValidDateRange(endDate, "endDate");
        //    if (startDate > endDate)
        //        throw new BadRequestException("endDate must be greater than startDate.");

        //    return new[] { startDate.ToUniversalTime().ToString("O"), endDate.ToUniversalTime().ToString("O") };
        //}

        [HttpGet]
        public async Task<IEnumerable<string>> Get([FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            ThrowIfNotInValidDateRange(startDate, "startDate");
            ThrowIfNotInValidDateRange(endDate, "endDate");
            if (startDate >= endDate)
                throw new BadRequestException("endDate must be greater than startDate.");

            var results = await _oldApi.GetUniqueTweets(startDate, endDate);
            return results;
            //return new[] { startDate.ToUniversalTime().ToString("O"), endDate.ToUniversalTime().ToString("O") };
        }
    }
}