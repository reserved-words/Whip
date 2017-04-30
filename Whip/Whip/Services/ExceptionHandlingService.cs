using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;
using Whip.ViewModels.Windows;

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

        public void Warn(Exception ex, string displayMessage = null)
        {
            LoopThroughInnerExceptions(ex, str => _loggingService.Warn(str));
            DisplayMessage(_messenger, "Warning", displayMessage);
        }

        public void Error(Exception ex, string displayMessage = null)
        {
            LoopThroughInnerExceptions(ex, str => _loggingService.Error(str));
            DisplayMessage(_messenger, "Error", displayMessage);
        }

        public void Fatal(Exception ex, string displayMessage = null)
        {
            LoopThroughInnerExceptions(ex, str => _loggingService.Fatal(str));

            string errorMessage = displayMessage ?? "An unexpected error occurred. The application will now shut down.";

            MessageBox.Show(errorMessage, "Fatal Application Error");

            Application.Current.Shutdown();
        }

        private void DisplayMessage(IMessenger messenger, string title, string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            messenger.Send(new ShowDialogMessage(new MessageViewModel(messenger, title, message)));
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
