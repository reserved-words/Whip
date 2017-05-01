using System;

namespace LastFmApi
{
    public class LastFmApiException : Exception
    {
        public LastFmApiException(string errorCode, string errorMessage)
            : base(errorMessage)
        {
            ParseErrorCode(errorCode);
        }

        public LastFmApiException(ErrorCode errorCode, string errorMessage)
            : base(errorMessage)
        {
            ErrorCode = errorCode;
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
