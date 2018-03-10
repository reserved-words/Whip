using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using Whip.Common.Interfaces;
using Whip.Windows;
using Whip.ViewModels.Messages;

namespace Whip.MessageHandlers
{
    public class DialogMessageHandler : IStartable
    {
        private readonly IMessenger _messenger;
        private readonly SynchronizationContext _synchronizationContext = SynchronizationContext.Current;
        private readonly Dictionary<Guid, Dialog> _dialogs = new Dictionary<Guid, Dialog>();

        private MiniPlayerWindow _miniPlayerWindow;

        public DialogMessageHandler(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public void Start()
        {
            _messenger.Register<ShowDialogMessage>(this, ShowDialog);
            _messenger.Register<HideDialogMessage>(this, HideDialog);
            _messenger.Register<ShowMiniPlayerMessage>(this, ShowMiniPlayer);
            _messenger.Register<HideMiniPlayerMessage>(this, HideMiniPlayer);
        }

        public void Stop()
        {
            _messenger.Unregister<ShowDialogMessage>(this, ShowDialog);
            _messenger.Unregister<HideDialogMessage>(this, HideDialog);
            _messenger.Unregister<ShowMiniPlayerMessage>(this, ShowMiniPlayer);
            _messenger.Unregister<HideMiniPlayerMessage>(this, HideMiniPlayer);
        }

        private void ShowDialog(ShowDialogMessage message)
        {
            _synchronizationContext.Send(state =>
            {
                Dialog dialog;
                if (!_dialogs.TryGetValue(message.ViewModel.Guid, out dialog))
                {
                    dialog = new Dialog
                    {
                        DataContext = message.ViewModel
                    };
                    _dialogs.Add(message.ViewModel.Guid, dialog);
                    dialog.ShowDialog();
                }
            }
            , null);    
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

        private void ShowMiniPlayer(ShowMiniPlayerMessage message)
        {
            _miniPlayerWindow = new MiniPlayerWindow
            {
                DataContext = message.MiniPlayerViewModel
            };
            Application.Current.MainWindow.Hide();
            _miniPlayerWindow.ShowDialog();
        }

        private void HideMiniPlayer(HideMiniPlayerMessage message)
        {
            _miniPlayerWindow.Close();
            _miniPlayerWindow = null;
            Application.Current.MainWindow.Show();
        }
    }
}
