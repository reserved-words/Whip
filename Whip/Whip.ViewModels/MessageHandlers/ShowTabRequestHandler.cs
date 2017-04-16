using GalaSoft.MvvmLight.Messaging;
using System;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.MessageHandlers
{
    public class ShowTabRequestHandler : IStartable, IShowTabRequestHandler
    {
        private readonly IMessenger _messenger;

        public event Action<Track> ShowEditTrackTab;
        public event Action ShowSettingsTab;

        public ShowTabRequestHandler(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public void Start()
        {
            _messenger.Register<EditTrackMessage>(this, OnEditTrack);
            _messenger.Register<EditSettingsMessage>(this, OnEditSettings);
        }

        public void Stop()
        {
            _messenger.Unregister<EditTrackMessage>(this, OnEditTrack);
            _messenger.Unregister<EditSettingsMessage>(this, OnEditSettings);
        }

        private void OnEditSettings(EditSettingsMessage message)
        {
            ShowSettingsTab?.Invoke();
        }

        private void OnEditTrack(EditTrackMessage message)
        {
            ShowEditTrackTab?.Invoke(message.Track);
        }
    }
}
