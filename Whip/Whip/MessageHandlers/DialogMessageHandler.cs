using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using Whip.ViewModels.Messages;
using Whip.Windows;

namespace Whip.MessageHandlers
{
    public class DialogMessageHandler : IMessageHandler
    {
        private readonly IMessenger _messenger;

        private readonly Dictionary<Guid, Dialog> _dialogs = new Dictionary<Guid, Dialog>();

        public DialogMessageHandler(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public void Start()
        {
            _messenger.Register<ShowDialogMessage>(this, ShowDialog);
            _messenger.Register<HideDialogMessage>(this, HideDialog);
        }
        
        public void Stop()
        {
            _messenger.Unregister<ShowDialogMessage>(this, ShowDialog);
            _messenger.Unregister<HideDialogMessage>(this, HideDialog);
        }

        private void ShowDialog(ShowDialogMessage message)
        {
            Dialog dialog;
            if (!_dialogs.TryGetValue(message.ViewModel.Guid, out dialog)){
                dialog = new Dialog
                {
                    DataContext = message.ViewModel
                };
                _dialogs.Add(message.ViewModel.Guid, dialog);
                dialog.Show();
            }
        }

        private void HideDialog(HideDialogMessage message)
        {
            Dialog dialog;
            if (!_dialogs.TryGetValue(message.Guid, out dialog))
            {
                _dialogs.Add(message.Guid, null);
            }
            else
            {
                dialog.Close();
                _dialogs[message.Guid] = null;
            }
        }
    }
}
