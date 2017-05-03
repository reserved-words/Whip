using LastFmApi;
using System;
using System.Threading.Tasks;
using Whip.Services.Interfaces;
using static Whip.Resources.Resources;

namespace Whip.LastFm
{
    public class LastFmMethodInterceptor : IAsyncMethodInterceptor
    {
        private readonly IUserSettings _userSettings;
        private readonly IExceptionHandlingService _errorHandler;

        public LastFmMethodInterceptor(IExceptionHandlingService errorHandler, IUserSettings userSettings)
        {
            _userSettings = userSettings;
            _errorHandler = errorHandler;
        }

        public async Task<T> TryMethod<T>(Task<T> task, T defaultReturnValue, string additionalErrorInfo = null)
        {
            if (_userSettings.LastFmStatus)
            {
                try
                {
                    var result = await task;
                    _userSettings.SetInternetStatus(true);
                    return result;
                }
                catch (Exception ex)
                {
                    HandleError(ex, additionalErrorInfo);
                }
            }

            return defaultReturnValue;
        }

        public async Task TryMethod(Task task, string additionalErrorInfo = null)
        {
            if (_userSettings.LastFmStatus)
            {
                try
                {
                    await task;
                    _userSettings.SetInternetStatus(true);
                }
                catch (Exception ex)
                {
                    HandleError(ex, additionalErrorInfo);
                }
            }
        }

        public void HandleError(Exception ex, string additionalInfo = "")
        {
            var lastFmException = GetLastFmException(ex);

            switch (lastFmException.ErrorCode)
            {
                case ErrorCode.InvalidApiKey:
                case ErrorCode.ServiceOffline:
                case ErrorCode.ApiKeySuspended:
                case ErrorCode.RateLimitExceeded:
                    _userSettings.TurnOffLastFm(GetUserFriendlyMessage(lastFmException.ErrorCode));
                    _errorHandler.Error(GetNewException(lastFmException, additionalInfo), LastFmOffErrorMessage);
                    break;
                case ErrorCode.ConnectionFailed:
                    _userSettings.SetInternetStatus(false);
                    _errorHandler.Warn(GetNewException(lastFmException, additionalInfo));
                    return;
                case ErrorCode.InvalidSessionKey:
                case ErrorCode.UnauthorizedToken:
                case ErrorCode.AuthenticationFailed:
                case ErrorCode.UserNotLoggedIn:
                    // Change this to add a way of resolving session issues instead
                    _userSettings.TurnOffLastFm(GetUserFriendlyMessage(lastFmException.ErrorCode));
                    _errorHandler.Error(GetNewException(lastFmException, additionalInfo), LastFmOffErrorMessage);
                    break;
                default:
                    _errorHandler.Warn(GetNewException(lastFmException, additionalInfo));
                    break;
            }
        }

        private Exception GetNewException(LastFmApiException ex, string additionalInfo)
        {
            var message = string.Format("Last.FM {0} : {1}{2}",
                ex.ErrorCode?.ToString() ?? string.Empty,
                ex.Message,
                string.IsNullOrEmpty(additionalInfo) ? string.Empty : $" ({additionalInfo})");

            return new Exception(message, ex);
        }

        private LastFmApiException GetLastFmException(Exception exception)
        {
            var ex = exception as LastFmApiException;

            while (exception != null && ex == null)
            {
                exception = exception.InnerException;
                ex = exception as LastFmApiException;
            }

            return ex;
        }

        private string GetUserFriendlyMessage(ErrorCode? code)
        {
            switch (code)
            {
                case ErrorCode.InvalidApiKey:
                    return "The API Key is invalid.";
                case ErrorCode.ServiceOffline:
                    return "The Last.FM service is offline";
                case ErrorCode.ApiKeySuspended:
                    return "The API Key is suspended";
                case ErrorCode.RateLimitExceeded:
                    return "The rate limit was exceeded";
                case ErrorCode.InvalidSessionKey:
                    return "The API Session Key is invalid";
                case ErrorCode.UnauthorizedToken:
                    return "The API token is not authorized";
                case ErrorCode.AuthenticationFailed:
                    return "Authentication failed";
                case ErrorCode.UserNotLoggedIn:
                    return "The user must be logged in";
            }

            return "An unexpected error was encountered";
        }
    }
}
