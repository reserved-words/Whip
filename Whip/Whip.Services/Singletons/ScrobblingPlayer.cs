using Whip.Common.Interfaces;
using Whip.Services.Interfaces;
using Whip.Common.Model;

namespace Whip.Services
{
    public class ScrobblingPlayer : IPlayer
    {
        private readonly IPlayer _player;
        private readonly IScrobblingRules _scrobblingRules;
        private readonly IScrobbler _scrobbler;
        private readonly ICurrentDateTime _currentDateTime;
        private readonly IPlayTimer _playingTimeTracker;

        private Track _currentTrack;

        public ScrobblingPlayer(IPlayer player, IScrobblingRules scrobblingRules, IScrobbler scrobbler, ICurrentDateTime currentDateTime,
            IPlayTimer playingTimeTracker)
        {
            _currentDateTime = currentDateTime;
            _player = player;
            _scrobblingRules = scrobblingRules;
            _scrobbler = scrobbler;
            _playingTimeTracker = playingTimeTracker;
        }

        public void Pause()
        {
            _player.Pause();
            _playingTimeTracker.Pause();
            _scrobbler.UpdateNowPlayingAsync(_currentTrack, _scrobblingRules.MinimumUpdateNowPlayingDuration);
        }

        public void Play(Track track)
        {
            _player.Play(track);

            var currentTime = _currentDateTime.Get();

            if (_currentTrack != null && _scrobblingRules.CanScrobble(_playingTimeTracker.TotalTrackDurationInSeconds,
                    _playingTimeTracker.SecondsOfTrackPlayed))
            {
                _scrobbler.ScrobbleAsync(_currentTrack, currentTime);
            }

            if (track != null)
            {
                _playingTimeTracker.StartNewTrack((int)track.Duration.TotalSeconds);
                _scrobbler.UpdateNowPlayingAsync(track, _playingTimeTracker.RemainingSeconds);
            }
            else
            {
                _scrobbler.UpdateNowPlayingAsync(_currentTrack, _scrobblingRules.MinimumUpdateNowPlayingDuration);
            }

            _currentTrack = track;
        }
        
        public void Resume()
        {
            _player.Resume();
            _playingTimeTracker.Resume();
            _scrobbler.UpdateNowPlayingAsync(_currentTrack, _playingTimeTracker.RemainingSeconds);
        }
        
        public void SkipToPercentage(double newPercentage)
        {
            _player.SkipToPercentage(newPercentage);
            _playingTimeTracker.SkipToPercentage(newPercentage);
            _scrobbler.UpdateNowPlayingAsync(_currentTrack, _playingTimeTracker.RemainingSeconds);
        }
    }
}
