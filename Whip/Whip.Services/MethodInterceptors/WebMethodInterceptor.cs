using System;
using System.Net;
using System.Net.Http;
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
            catch (ServiceUnavailableException ex)
            {
                Handle(type, ex, additionalErrorInfo);
            }
            catch (ServiceAuthenticationException ex)
            {
                Handle(type, ex, additionalErrorInfo);
            }
            catch (ServiceException ex)
            {
                Handle(type, ex, additionalErrorInfo);
            }
            catch (WebException ex)
            {
                Handle(ex, additionalErrorInfo);
            }
            catch (HttpRequestException ex)
            {
                Handle(ex, additionalErrorInfo);
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
            catch (ServiceUnavailableException ex)
            {
                Handle(type, ex, additionalErrorInfo);
            }
            catch (ServiceAuthenticationException ex)
            {
                Handle(type, ex, additionalErrorInfo);
            }
            catch (ServiceException ex)
            {
                Handle(type, ex, additionalErrorInfo);
            }
            catch (WebException ex)
            {
                Handle(ex, additionalErrorInfo);
            }
            catch (HttpRequestException ex)
            {
                Handle(ex, additionalErrorInfo);
            }
        }

        private void Handle(WebServiceType type, ServiceUnavailableException ex, string additionalErrorInfo)
        {
            _servicesStatus.SetStatus(type, false, ex.Message);
            _errorHandler.Warn(new Exception(additionalErrorInfo, ex.InnerException));
        }

        private void Handle(WebServiceType type, ServiceAuthenticationException ex, string additionalErrorInfo)
        {
            _servicesStatus.SetStatus(type, false, ex.Message);
            _errorHandler.Error(new Exception(additionalErrorInfo, ex), $"Authentication error for {type} service:" 
                + Environment.NewLine 
                + Environment.NewLine 
                + ex.Message);
        }

        private void Handle(WebServiceType type, ServiceException ex, string additionalErrorInfo)
        {
            _servicesStatus.SetStatus(type, true);
            _errorHandler.Warn(new Exception(additionalErrorInfo, ex));
        }

        private void Handle(WebException ex, string additionalErrorInfo)
        {
            _servicesStatus.SetStatus(WebServiceType.Web, false);
            _errorHandler.Warn(new Exception(additionalErrorInfo, ex));
        }

        private void Handle(HttpRequestException ex, string additionalErrorInfo)
        {
            var inner = ex.InnerException as WebException;

            if (inner == null)
                throw ex;

            Handle(inner, additionalErrorInfo);
        }
    }
}
