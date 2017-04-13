using GalaSoft.MvvmLight.Messaging;
using System;
using System.Timers;
using Whip.Common;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Common.Singletons;

namespace Whip.ViewModels.MessageHandlers
{
    public class TrackChangeCoordinator : IStartable, IPlayerUpdate
    {
        private readonly System.Threading.SynchronizationContext _synchronizationContext = System.Threading.SynchronizationContext.Current;
        private readonly Timer _timer = new Timer(ApplicationSettings.TrackChangeDelay);

        private readonly Playlist _playlist;
        private readonly IPlayer _player;
        private readonly IMessenger _messenger;

        private Track _track;

        public event Action<Track> NewTrackStarted;

        public TrackChangeCoordinator(Playlist playlist, IPlayer player, IMessenger messenger)
        {
            _playlist = playlist;
            _player = player;
            _messenger = messenger;

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
