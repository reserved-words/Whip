using System.Threading.Tasks;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Web.Interfaces;

namespace Whip.Web.Services
{
    public class WebScrobblingPlayer : IPlayer
    {
        private readonly ICurrentDateTime _currentDateTime;
        private readonly IScrobblingRules _scrobblingRules;
        private readonly IScrobbler _scrobbler;
        private readonly IPlayProgress _playProgress;

        private Track _currentTrack;

        public WebScrobblingPlayer(ICurrentDateTime currentDateTime, IScrobblingRules scrobblingRules, IScrobbler scrobbler, IPlayProgress playProgress)
        {
            _currentDateTime = currentDateTime;
            _playProgress = playProgress;
            _scrobblingRules = scrobblingRules;
            _scrobbler = scrobbler;
        }

        public void Pause()
        {
            _scrobbler.UpdateNowPlayingAsync(_currentTrack, _scrobblingRules.MinimumUpdateNowPlayingDuration);
        }

        public void Play(Track track)
        {
            if (_currentTrack != null)
            {
                if (CanScrobbleTrack(_playProgress.PreviousDuration, _playProgress.PreviousSecondsPlayed))
                {
                    _scrobbler.ScrobbleAsync(_currentTrack, _currentDateTime.Get());
                }
            }

            if (track != null)
            {
                _scrobbler.UpdateNowPlayingAsync(track, _playProgress.CurrentSecondsRemaining);
            }
            else
            {
                _scrobbler.UpdateNowPlayingAsync(_currentTrack, _scrobblingRules.MinimumUpdateNowPlayingDuration);
            }

            _currentTrack = track;
        }

        public void Resume()
        {
            _scrobbler.UpdateNowPlayingAsync(_currentTrack, _playProgress.CurrentSecondsRemaining);
        }

        public void SkipToPercentage(double newPercentage)
        {
            _scrobbler.UpdateNowPlayingAsync(_currentTrack, _playProgress.CurrentSecondsRemaining);
        }

        private bool CanScrobbleTrack(int duration, int secondsPlayed)
        {
            return _scrobblingRules.CanScrobble(duration, secondsPlayed);
        }

        public void Stop()
        {
            if (_currentTrack == null)
                return;

            if (CanScrobbleTrack(_playProgress.PreviousDuration, _playProgress.PreviousSecondsPlayed))
            {
                var scrobble = Task.Run(() => _scrobbler.ScrobbleAsync(_currentTrack, _currentDateTime.Get())).Result;
            }

            var updateNowPlaying = Task.Run(
                () => _scrobbler.UpdateNowPlayingAsync(_currentTrack, _scrobblingRules.MinimumUpdateNowPlayingDuration)).Result;
        }
    }
}