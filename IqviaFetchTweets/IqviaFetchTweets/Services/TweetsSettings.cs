using IqviaFetchTweets.Interfaces;
using Microsoft.Extensions.Configuration;

namespace IqviaFetchTweets.Services
{
    /// <summary>
    /// Stores settings for the TweetsController. The members of this class
    /// should be set once by the Startup class.
    /// </summary>
    public sealed class TweetsSettings : ITweetsSettings
    {
        public TweetsSettings(IConfiguration c)
        {
            int.TryParse(c["Tweets:MaxYearsIntoPast"], out var maxYearsBack);
            MaxYearsIntoPast = maxYearsBack;
        }

        public int MaxYearsIntoPast { get; }
    }
}
