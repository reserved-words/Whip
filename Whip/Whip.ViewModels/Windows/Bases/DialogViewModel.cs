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

        public DialogViewModel(IMessenger messenger, string title, IconType iconType, Action callback = null, bool startInCenter = true)
        {
            _messenger = messenger;
            _callback = callback;

            Title = title;
            Icon = iconType.ToString();
            Guid = Guid.NewGuid();
            StartInCenter = startInCenter;
        }

        public Guid Guid { get; }
        public string Title { get; }
        public string Icon { get; }
        public bool StartInCenter { get; }

        protected virtual void Close()
        {
            _messenger.Send(new HideDialogMessage(Guid));
            
            _callback?.Invoke();
        }
    }
}
