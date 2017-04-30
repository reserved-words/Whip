using System;

namespace LastFmApi
{
    public enum ErrorCode
    {
        InvalidService = 2,
        InvalidMethod = 3,
        AuthenticationFailed = 4,
        InvalidFormat = 5,
        InvalidParameters = 6,
        InvalidResourceSpecified = 7,
        OperationFailed = 8,
        InvalidSessionKey = 9,
        InvalidApiKey = 10,
        ServiceOffline = 11,
        SubscribersOnly = 12,
        InvalidMethodSignature = 13,
        UnauthorizedToken = 14,
        ItemStreamingUnavailable = 15,
        ServiceTemporarilyUnavailable = 16,
        UserNotLoggedIn = 17,
        TrialExpired = 18,
        NotEnoughContent = 20,
        NotEnoughMembers = 21,
        NotEnoughFans = 22,
        NotEnoughNeighbours = 23,
        NoPeakRadio = 24,
        RadioNotFound = 25,
        ApiKeySuspended = 26,
        Deprecated = 27,
        RateLimitExceeded = 29,
        HttpErrorResponse = 998,
        ConnectionFailed = 999
    }

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
