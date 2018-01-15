using System;
using System.Net;
using System.Threading.Tasks;
using Whip.Common.Enums;
using Whip.Common.Exceptions;
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

        public async Task<T> TryMethod<T>(Task<T> task, T defaultReturnValue, WebServiceType type, string additionalErrorInfo = null)
        {
            try
            {
                _servicesStatus.SetStatus(WebServiceType.Web, true);
                _servicesStatus.SetStatus(type, true);
                return await task;
            }
            catch (WebException ex)
            {
                _servicesStatus.SetStatus(WebServiceType.Web, false);
                _errorHandler.Warn(new Exception(additionalErrorInfo, ex));
            }
            catch (WebServiceUnavailableException ex)
            {
                _servicesStatus.SetStatus(type, false, ex.Message);
                _errorHandler.Warn(new Exception(additionalErrorInfo, ex.InnerException));
            }

            return defaultReturnValue;
        }

        public async Task TryMethod(Task task, WebServiceType type, string additionalErrorInfo = null)
        {
            try
            {
                _servicesStatus.SetStatus(WebServiceType.Web, true);
                _servicesStatus.SetStatus(type, true);
                await task;
            }
            catch (WebException ex)
            {
                _servicesStatus.SetStatus(WebServiceType.Web, false);
                _errorHandler.Warn(new Exception(additionalErrorInfo, ex));
            }
            catch (WebServiceUnavailableException ex)
            {
                _servicesStatus.SetStatus(type, false, ex.Message);
                _errorHandler.Warn(new Exception(additionalErrorInfo, ex.InnerException));
            }
        }
    }
}
