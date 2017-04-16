using GalaSoft.MvvmLight.Messaging;
using System;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.MessageHandlers
{
    public class EditTrackRequestHandler : IStartable, IEditTrackRequester
    {
        private readonly IMessenger _messenger;

        public event Action<Track> RequestAccepted;

        public EditTrackRequestHandler(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public void Start()
        {
            _messenger.Register<EditTrackMessage>(this, OnEditTrack);
        }

        public void Stop()
        {
            _messenger.Unregister<EditTrackMessage>(this, OnEditTrack);
        }

        private void OnEditTrack(EditTrackMessage message)
        {
            RequestAccepted?.Invoke(message.Track);
        }
    }
}
