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
        private readonly ILoggingService _logger;

        private Track _track;

        public event Action<Track> NewTrackStarted;

        public TrackChangeCoordinator(IPlaylist playlist, IPlayer player, IConfigSettings configSettings, ILoggingService logger)
        {
            _playlist = playlist;
            _player = player;
            _logger = logger;

            _timer = new Timer(configSettings.TrackChangeDelay);

            _timer.Elapsed += _timer_Elapsed;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _synchronizationContext.Send(state =>
            {
                _logger.Info("Stopping timer");
                _timer.Stop();
                _logger.Info("Start playing " + _track.Title);
                _player.Play(_track);
                _logger.Info("Invoke new track started");
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
            _logger.Info("TrackChangeCoordinator: Track changed to " + track);

            _synchronizationContext.Send(state =>
            {
                _timer.Stop();
                _logger.Info("Pausing player");
                _player.Pause();
                _logger.Info("Setting track to " + track);
                _track = track;
                _timer.Start();
            }
            , null);
        }
    }
}
