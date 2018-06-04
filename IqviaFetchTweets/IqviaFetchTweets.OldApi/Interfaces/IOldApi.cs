using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IqviaFetchTweets.OldApi.Interfaces
{
    /// <summary>
    /// Defines the interface OldApi, which is used to retreive
    /// tweets from the old Iqvia API.
    /// </summary>
    public interface IOldApi
    {
        /// <summary>
        /// Returns all unique tweets from the old Iqvia API.
        /// </summary>
        /// <param name="startDate">Starting date range.</param>
        /// <param name="endDate">Ending date range.</param>
        /// <returns></returns>
        Task<List<string>> GetUniqueTweets(DateTime startDate, DateTime endDate);
    }
}
