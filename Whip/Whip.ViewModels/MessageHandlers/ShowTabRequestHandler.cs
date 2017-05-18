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
        public event Action<CriteriaPlaylist> ShowEditCriteriaPlaylistTab;
        public event Action<OrderedPlaylist> ShowEditOrderedPlaylistTab;
        public event Action ShowSettingsTab;

        public ShowTabRequestHandler(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public void Start()
        {
            _messenger.Register<EditTrackMessage>(this, OnEditTrack);
            _messenger.Register<EditSettingsMessage>(this, OnEditSettings);
            _messenger.Register<EditCriteriaPlaylistMessage>(this, OnEditCriteriaPlaylist);
            _messenger.Register<EditOrderedPlaylistMessage>(this, OnEditOrderedPlaylist);
        }

        public void Stop()
        {
            _messenger.Unregister<EditTrackMessage>(this, OnEditTrack);
            _messenger.Unregister<EditSettingsMessage>(this, OnEditSettings);
            _messenger.Unregister<EditCriteriaPlaylistMessage>(this, OnEditCriteriaPlaylist);
            _messenger.Unregister<EditOrderedPlaylistMessage>(this, OnEditOrderedPlaylist);
        }

        private void OnEditSettings(EditSettingsMessage message)
        {
            ShowSettingsTab?.Invoke();
        }

        private void OnEditTrack(EditTrackMessage message)
        {
            if (message.Track == null)
                return;

            ShowEditTrackTab?.Invoke(message.Track);
        }

        private void OnEditCriteriaPlaylist(EditCriteriaPlaylistMessage message)
        {
            ShowEditCriteriaPlaylistTab?.Invoke(message.Playlist);
        }

        private void OnEditOrderedPlaylist(EditOrderedPlaylistMessage message)
        {
            ShowEditOrderedPlaylistTab?.Invoke(message.Playlist);
        }
    }
}
