using GalaSoft.MvvmLight.Messaging;
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
            _playlist.CurrentTrackChanged += OnCurrentTrackChanged;

            _messenger.Register<PausePlayerMessage>(this, OnPausePlayer);
            _messenger.Register<ResumePlayerMessage>(this, OnResumePlayer);
        }

        public void Stop()
        {
            _playlist.ListUpdated -= OnPlaylistUpdated;
            _playlist.CurrentTrackChanged -= OnCurrentTrackChanged;

            _messenger.Unregister<PausePlayerMessage>(this, OnPausePlayer);
            _messenger.Unregister<ResumePlayerMessage>(this, OnResumePlayer);
        }

        private void OnCurrentTrackChanged(Track track)
        {
            _player.Play(track.FullFilepath); 
        }

        private void OnPausePlayer(PausePlayerMessage message)
        {
            _player.Pause();            
        }

        private void OnPlaylistUpdated()
        {
            _playlist.MoveNext();
        }

        private void OnResumePlayer(ResumePlayerMessage message)
        {
            _player.Resume();
        }
    }
}
