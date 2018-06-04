using System;

namespace IqviaFetchTweets.Exceptions
{
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(string message = "Unknown bad request") 
            : base(message)
        {

        }
    }
}
