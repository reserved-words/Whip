using System;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Services.Interfaces.Singletons;

namespace Whip.ViewModels.MessageHandlers
{
    public class TrackChangeCoordinator : IStartable, IPlayerUpdate
    {
        private readonly IPlaylist _playlist;
        private readonly IPlayer _player;

        public event Action<Track> NewTrackStarted;

        public TrackChangeCoordinator(IPlaylist playlist, IPlayer player)
        {
            _playlist = playlist;
            _player = player;
        }

        public void Start()
        {
            _playlist.CurrentTrackChanged += OnCurrentTrackChanged;
        }

        public void Stop()
        {
            _playlist.CurrentTrackChanged -= OnCurrentTrackChanged;
        }

        private void OnCurrentTrackChanged(Track track)
        {
            _player.Play(track);
            NewTrackStarted?.Invoke(track);
        }
    }
}
