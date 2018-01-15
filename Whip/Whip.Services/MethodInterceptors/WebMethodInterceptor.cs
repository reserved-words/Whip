using System;
using System.Net;
using System.Threading.Tasks;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.Services
{
    public class WebMethodInterceptor : IAsyncMethodInterceptor
    {
        private readonly IWebServicesStatus _servicesStatus;
        private readonly IExceptionHandlingService _errorHandler;

        public WebMethodInterceptor(IExceptionHandlingService errorHandler, IWebServicesStatus servicesStatus)
        {
            _servicesStatus = servicesStatus;
            _errorHandler = errorHandler;
        }

        public async Task<T> TryMethod<T>(Task<T> task, T defaultReturnValue, string additionalErrorInfo = null)
        {
            try
            {
                var result = await task;
                _servicesStatus.SetInternetStatus(true);
                return result;
            }
            catch (WebException ex)
            {
                _servicesStatus.SetInternetStatus(false);
                _errorHandler.Warn(new Exception(additionalErrorInfo, ex));
            }

            return defaultReturnValue;
        }

        public async Task TryMethod(Task task, string additionalErrorInfo = null)
        {
            try
            {
                await task;
                _servicesStatus.SetInternetStatus(true);
            }
            catch (WebException ex)
            {
                _servicesStatus.SetInternetStatus(false);
                _errorHandler.Warn(new Exception(additionalErrorInfo, ex));
            }
        }
    }
}
