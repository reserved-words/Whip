using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class ExceptionHandlingService : IExceptionHandlingService
    {
        private readonly ILoggingService _loggingService;
        private readonly IMessenger _messenger;

        public ExceptionHandlingService(ILoggingService loggingService, IMessenger messenger)
        {
            _loggingService = loggingService;
            _messenger = messenger;
        }

        public void Warn(Exception ex)
        {
            // Display warning message?
            LoopThroughInnerExceptions(ex, str => _loggingService.Warn(str));
        }

        public void Error(Exception ex)
        {
            // Display error message
            LoopThroughInnerExceptions(ex, str => _loggingService.Error(str));
        }

        public void Fatal(Exception ex)
        {
            LoopThroughInnerExceptions(ex, str => _loggingService.Fatal(str));

            string errorMessage = "An unexpected error occurred. The application will now shut down.";

            MessageBox.Show(errorMessage, "Fatal Application Error");

            Application.Current.Shutdown();
        }

        private void LoopThroughInnerExceptions(Exception ex, Action<string> action)
        {
            while (ex != null)
            {
                action(ex.GetType().Name);
                action(ex.Message);

                ex = ex.InnerException;
            }
        }
    }
}
