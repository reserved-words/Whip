using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using Whip.Common;
using Whip.ViewModels.Windows;

namespace Whip.ViewModels.Messages
{
    public enum MessageType { Info, Warning, Error }

    public class ShowDialogMessage
    {
        private readonly Dictionary<MessageType, IconType> _icons = new Dictionary<MessageType, IconType>
        {
            { MessageType.Info, IconType.InfoCircle },
            { MessageType.Warning, IconType.ExclamationTriangle },
            { MessageType.Error, IconType.TimesRectangle }
        };

        public ShowDialogMessage(DialogViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public ShowDialogMessage(IMessenger messenger, MessageType messageType, string title, string text)
        {
            ViewModel = new MessageViewModel(messenger, title, _icons[messageType], text);
        }

        public DialogViewModel ViewModel { get; set; }
    }
}
