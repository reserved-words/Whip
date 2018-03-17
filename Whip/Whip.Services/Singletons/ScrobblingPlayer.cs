using System.Threading.Tasks;
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
            _playProgressTracker.Stop();
            _scrobbler.UpdateNowPlayingAsync(_currentTrack, _scrobblingRules.MinimumUpdateNowPlayingDuration);
        }

        public void Play(Track track)
        {
            _player.Play(track);

            if (_currentTrack != null)
            {
                _playProgressTracker.Stop();
                ScrobbleCurrentTrack();
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

        private bool CanScrobbleCurrentTrack()
        {
            return _scrobblingRules.CanScrobble(_playProgressTracker.TotalTrackDurationInSeconds,
                _playProgressTracker.SecondsOfTrackPlayed);
        }

        private void ScrobbleCurrentTrack()
        {
            if (CanScrobbleCurrentTrack())
            {
                _scrobbler.ScrobbleAsync(_currentTrack, _currentDateTime.Get());
            }
        }

        public int GetVolumePercentage()
        {
            return _player.GetVolumePercentage();
        }

        public void Mute()
        {
            _player.Mute();
        }

        public void SetVolumePercentage(int volume)
        {
            _player.SetVolumePercentage(volume);
        }

        public void Unmute()
        {
            _player.Unmute();
        }

        public void Stop()
        {
            _player.Stop();
            _playProgressTracker.Stop();

            if (CanScrobbleCurrentTrack())
            {
                var scrobble = Task.Run(() => _scrobbler.ScrobbleAsync(_currentTrack, _currentDateTime.Get())).Result;
            }

            var updateNowPlaying = Task.Run(
                () => _scrobbler.UpdateNowPlayingAsync(_currentTrack, _scrobblingRules.MinimumUpdateNowPlayingDuration)).Result;
        }
    }
}
