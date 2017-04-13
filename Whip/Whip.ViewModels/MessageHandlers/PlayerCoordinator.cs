using GalaSoft.MvvmLight.Messaging;
using System;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.MessageHandlers
{
    public class PlayerCoordinator : IStartable
    {
        private readonly Playlist _playlist;
        private readonly IPlayer _player;
        private readonly IMessenger _messenger;

        public PlayerCoordinator(Playlist playlist, IPlayer player, IMessenger messenger)
        {
            _playlist = playlist;
            _player = player;
            _messenger = messenger;
        }

        public void Start()
        {
            _playlist.ListUpdated += OnPlaylistUpdated;

            _messenger.Register<PausePlayerMessage>(this, OnPausePlayer);
            _messenger.Register<ResumePlayerMessage>(this, OnResumePlayer);
            _messenger.Register<SkipToPercentageMessage>(this, OnSkipToPercentage);
            _messenger.Register<PlayNextMessage>(this, OnPlayNext);
            _messenger.Register<PlayPreviousMessage>(this, OnPlayPrevious);
        }

        public void Stop()
        {
            _playlist.ListUpdated -= OnPlaylistUpdated;

            _messenger.Unregister<PausePlayerMessage>(this, OnPausePlayer);
            _messenger.Unregister<ResumePlayerMessage>(this, OnResumePlayer);
            _messenger.Unregister<SkipToPercentageMessage>(this, OnSkipToPercentage);
            _messenger.Unregister<PlayNextMessage>(this, OnPlayNext);
            _messenger.Unregister<PlayPreviousMessage>(this, OnPlayPrevious);
        }

        private void OnPausePlayer(PausePlayerMessage message)
        {
            _player.Pause();            
        }

        private void OnPlayNext(PlayNextMessage message)
        {
            _playlist.MoveNext();
        }

        private void OnPlayPrevious(PlayPreviousMessage message)
        {
            _playlist.MovePrevious();
        }

        private void OnPlaylistUpdated()
        {
            _playlist.MoveNext();
        }

        private void OnResumePlayer(ResumePlayerMessage message)
        {
            _player.Resume();
        }

        private void OnSkipToPercentage(SkipToPercentageMessage message)
        {
            _player.SkipToPercentage(message.NewPercentage);
        }
    }
}
