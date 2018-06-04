using System;

namespace IqviaFetchTweets.OldApi.DTO
{
    /// <summary>
    /// Defines a Tweet object.
    /// </summary>
    public class Tweet
    {
        /// <summary>
        /// Tweet ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Tweet timestamp.
        /// </summary>
        public DateTime Stamp { get; set; }

        /// <summary>
        /// Tweet text.
        /// </summary>
        public string Text { get; set; }
    }
}
