using System;
using System.Net;
using System.Threading.Tasks;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class WebMethodInterceptor : IAsyncMethodInterceptor
    {
        private readonly IUserSettings _userSettings;
        private readonly IExceptionHandlingService _errorHandler;

        public WebMethodInterceptor(IExceptionHandlingService errorHandler, IUserSettings userSettings)
        {
            _userSettings = userSettings;
            _errorHandler = errorHandler;
        }

        public async Task<T> TryMethod<T>(Task<T> task, T defaultReturnValue, string additionalErrorInfo = null)
        {
            try
            {
                var result = await task;
                _userSettings.SetInternetStatus(true);
                return result;
            }
            catch (WebException ex)
            {
                _userSettings.SetInternetStatus(false);
                _errorHandler.Warn(new Exception(additionalErrorInfo, ex));
            }

            return defaultReturnValue;
        }

        public Task TryMethod(Task task, string additionalErrorInfo = null)
        {
            throw new NotImplementedException();
        }
    }
}
