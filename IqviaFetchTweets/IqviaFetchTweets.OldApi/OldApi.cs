using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IqviaFetchTweets.OldApi.DTO;
using IqviaFetchTweets.OldApi.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace IqviaFetchTweets.OldApi
{
    /// <summary>
    /// Interface to the old, bad IQVIA API.
    /// </summary>
    public sealed class OldApi : IOldApi
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly string _resourceUri;
        private readonly int _maxResults;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="resourceUri">Base URI to get tweets.</param>
        /// <param name="maxResults">The maximum number of tweets that this API can return.</param>
        public OldApi(string resourceUri, int maxResults)
        {
            _resourceUri = resourceUri;
            _maxResults = maxResults;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="c">Configuration settings.</param>
        public OldApi(IConfiguration c)
        {
            _resourceUri = c["OldApi:BaseUri"];
            int.TryParse(c["OldApi:MaxResults"], out var maxResults);
            _maxResults = maxResults;
        }

        private async Task<List<Tweet>> FetchFromServer(string uri)
        {
            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var raw = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Tweet>>(raw);
            }

            return new List<Tweet>();
        }

        public async Task<List<string>> GetUniqueTweets(DateTime startDate, DateTime endDate)
        {
            var uniqueTweets = new Dictionary<string, Tweet>();
            var start = startDate;
            var end = endDate;
            List<Tweet> tweets;

            do
            {
                var uri = $"{_resourceUri}?startDate={start.ToUniversalTime().ToString("O")}&endDate={end.ToUniversalTime().ToString("O")}";
                tweets = await FetchFromServer(uri);
                Console.WriteLine(uri);
                foreach (var tweet in tweets)
                {
                    uniqueTweets.TryAdd(tweet.Text.Trim(), tweet);

                    if (tweet.Stamp.ToUniversalTime() > start.ToUniversalTime())
                        start = tweet.Stamp.ToUniversalTime();
                }
            } while (tweets.Count >= _maxResults && start < endDate);

            return uniqueTweets.Keys.ToList();
        }
    }
}
