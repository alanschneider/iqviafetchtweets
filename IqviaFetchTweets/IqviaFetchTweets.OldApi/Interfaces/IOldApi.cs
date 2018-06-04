using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IqviaFetchTweets.OldApi.Interfaces
{
    public interface IOldApi
    {
        Task<List<string>> GetUniqueTweets(DateTime startDate, DateTime endDate);
    }
}
