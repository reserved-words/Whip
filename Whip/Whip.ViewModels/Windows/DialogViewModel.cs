using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using Whip.Common;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.Windows
{
    public class DialogViewModel : ViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly Action _callback;

        public DialogViewModel(IMessenger messenger, string title, IconType iconType, Action callback = null)
        {
            _messenger = messenger;
            _callback = callback;

            Title = title;
            Icon = iconType.ToString();
            Guid = Guid.NewGuid();
        }

        public Guid Guid { get; private set; }

        public string Title { get; private set; }

        public string Icon { get; private set; }

        protected virtual void Close()
        {
            _messenger.Send(new HideDialogMessage(Guid));
            
            _callback?.Invoke();
        }
    }
}
