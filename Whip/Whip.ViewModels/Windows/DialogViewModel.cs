using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using Whip.Common.Interfaces;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.Windows
{
    public class DialogViewModel : ViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly Action _callback;

        public DialogViewModel(IMessenger messenger, string title, Action callback = null)
        {
            _messenger = messenger;
            _callback = callback;

            Title = title;
            Guid = Guid.NewGuid();
        }

        public Guid Guid { get; private set; }

        public string Title { get; private set; }

        protected virtual void Close()
        {
            _messenger.Send(new HideDialogMessage(Guid));
            
            _callback?.Invoke();
        }
    }
}
