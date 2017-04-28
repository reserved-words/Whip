using System;

namespace LastFmApi
{
    public enum ErrorCode
    {
        ErrorDoesNotExist = 1,
        InvalidService = 2,
        InvalidMethod = 3,
        AuthenticationFailed = 4,
        InvalidFormat = 5,
        InvalidParameters = 6
    }

    public class LastFmApiException : Exception
    {
        public LastFmApiException(string errorCode, string errorMessage)
            : base(errorMessage)
        {
            ParseErrorCode(errorCode);
        }

        public ErrorCode? ErrorCode { get; private set; }

        private void ParseErrorCode(string str)
        {
            ErrorCode code;
            if (Enum.TryParse(str, out code))
            {
                ErrorCode = code;
            } 
        }
    }
}
