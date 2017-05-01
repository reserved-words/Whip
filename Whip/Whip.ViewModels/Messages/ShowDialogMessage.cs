using GalaSoft.MvvmLight.Messaging;
using Whip.ViewModels.Windows;

namespace Whip.ViewModels.Messages
{
    public enum MessageType { Info, Warning, Error }

    public class ShowDialogMessage
    {
        public ShowDialogMessage(DialogViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public ShowDialogMessage(IMessenger messenger, MessageType messageType, string title, string text)
        {
            ViewModel = new MessageViewModel(messenger, title, text);
        }

        public DialogViewModel ViewModel { get; set; }
    }
}
