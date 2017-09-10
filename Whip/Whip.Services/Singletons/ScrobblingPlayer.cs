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
        private readonly IPlayProgressTracker _playProgressTracker;

        private Track _currentTrack;

        public ScrobblingPlayer(IPlayer player, IScrobblingRules scrobblingRules, IScrobbler scrobbler, ICurrentDateTime currentDateTime,
            IPlayProgressTracker playProgressTracker)
        {
            _currentDateTime = currentDateTime;
            _player = player;
            _scrobblingRules = scrobblingRules;
            _scrobbler = scrobbler;
            _playProgressTracker = playProgressTracker;
        }

        public void Pause()
        {
            _player.Pause();
            _playProgressTracker.Pause();
            _scrobbler.UpdateNowPlayingAsync(_currentTrack, _scrobblingRules.MinimumUpdateNowPlayingDuration);
        }

        public void Play(Track track)
        {
            _player.Play(track);

            var currentTime = _currentDateTime.Get();

            if (_currentTrack != null && _scrobblingRules.CanScrobble(_playProgressTracker.TotalTrackDurationInSeconds,
                    _playProgressTracker.SecondsOfTrackPlayed))
            {
                _scrobbler.ScrobbleAsync(_currentTrack, currentTime);
            }

            if (track != null)
            {
                _playProgressTracker.StartNewTrack((int)track.Duration.TotalSeconds);
                _scrobbler.UpdateNowPlayingAsync(track, _playProgressTracker.RemainingSeconds);
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
            _playProgressTracker.Resume();
            _scrobbler.UpdateNowPlayingAsync(_currentTrack, _playProgressTracker.RemainingSeconds);
        }
        
        public void SkipToPercentage(double newPercentage)
        {
            _player.SkipToPercentage(newPercentage);
            _playProgressTracker.SkipToPercentage(newPercentage);
            _scrobbler.UpdateNowPlayingAsync(_currentTrack, _playProgressTracker.RemainingSeconds);
        }
    }
}
