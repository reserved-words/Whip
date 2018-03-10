using GalaSoft.MvvmLight.Messaging;
using System;
using System.Text;
using System.Windows;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;

namespace Whip.Services
{
    public class ExceptionHandlingService : IExceptionHandlingService
    {
        private readonly ILoggingService _loggingService;
        private readonly IMessenger _messenger;
        private readonly IErrorLoggingService _errorLogger;

        public ExceptionHandlingService(ILoggingService loggingService, IMessenger messenger,
            IErrorLoggingService errorLogger)
        {
            _loggingService = loggingService;
            _messenger = messenger;
            _errorLogger = errorLogger;
        }

        public void Warn(Exception ex, string displayMessage = null)
        {
            LoopThroughInnerExceptions(ex, str => _loggingService.Warn(str));
            DisplayMessage(MessageType.Warning, "Warning", displayMessage);
        }

        public void Error(Exception ex, string displayMessage = null)
        {
            LoopThroughInnerExceptions(ex, str => _loggingService.Error(str));
            DisplayMessage(MessageType.Error, "Error", displayMessage);
        }

        public void Fatal(Exception ex, string displayMessage = null)
        {
            LoopThroughInnerExceptions(ex, str => _loggingService.Fatal(str));
            string errorMessage = displayMessage ?? "An unexpected error occurred. The application will now shut down.";
            MessageBox.Show(errorMessage, "Fatal Application Error");
            Application.Current.Shutdown();
        }

        private void DisplayMessage(MessageType messageType, string title, string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            _messenger.Send(new ShowDialogMessage(_messenger, messageType, title, message));
        }

        private void LoopThroughInnerExceptions(Exception ex, Action<string> action)
        {
            while (ex != null)
            {
                _errorLogger.Log(ex);
                action(ex.Message);
                ex = ex.InnerException;
            }
        }
    }
}
