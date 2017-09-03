using GalaSoft.MvvmLight.Messaging;
using System;
using System.Timers;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.ViewModels.MessageHandlers
{
    public class TrackChangeCoordinator : IStartable, IPlayerUpdate
    {
        private readonly System.Threading.SynchronizationContext _synchronizationContext = System.Threading.SynchronizationContext.Current;
        private readonly Timer _timer;

        private readonly IPlaylist _playlist;
        private readonly IPlayer _player;
        private readonly IConfigSettings _configSettings;

        private Track _track;

        public event Action<Track> NewTrackStarted;

        public TrackChangeCoordinator(IPlaylist playlist, IPlayer player, IConfigSettings configSettings)
        {
            _playlist = playlist;
            _player = player;
            _configSettings = configSettings;

            _timer = new Timer(_configSettings.TrackChangeDelay);

            _timer.Elapsed += _timer_Elapsed;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _synchronizationContext.Send(state =>
            {
                _timer.Stop();
                _player.Play(_track);
                NewTrackStarted?.Invoke(_track);
            }
            , null);
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
            _synchronizationContext.Send(state =>
            {
                _timer.Stop();
                _player.Pause();
                _track = track;
                _timer.Start();
            }
            , null);
        }
    }
}
