using System;
using Whip.Common.Interfaces;
using Whip.Services.Interfaces;
using Whip.Common.Model;

namespace Whip.Services
{
    public class ScrobblingPlayer : IPlayer
    {
        private readonly IPlayer _player;
        private readonly IScrobblingRulesService _scrobblingRulesService;
        private readonly IScrobblingService _scrobblingService;
        private readonly ILoggingService _logger;

        private Track _currentTrack;

        private double _totalSeconds;
        private double _secondsPlayed;
        private double _remainingSeconds;

        private DateTime _pausedAt;
        private DateTime _playingAt;

        public ScrobblingPlayer(IPlayer player, IScrobblingRulesService scrobblingRulesService, IScrobblingService scrobblingService,
            ILoggingService logger)
        {
            _player = player;
            _scrobblingRulesService = scrobblingRulesService;
            _scrobblingService = scrobblingService;
            _logger = logger;
        }

        public void Pause()
        {
            _pausedAt = DateTime.Now;
            _secondsPlayed = _secondsPlayed + (_pausedAt - _playingAt).TotalSeconds;
            UpdateRemainingSeconds();

            _player.Pause();

            _scrobblingService.UpdateNowPlayingAsync(_currentTrack, 30);
        }

        public void Play(Track track)
        {
            _logger.Info("ScrobblingPlayer: Play " + track.Title);

            var previousTrack = _currentTrack;
            _totalSeconds = track?.Duration.TotalSeconds ?? 0;

            UpdateCurrentTrack(track);

            _player.Play(track);

            ProcessTrackChange(previousTrack);
        }
        
        public void Resume()
        {
            _playingAt = DateTime.Now;

            _player.Resume();

            _scrobblingService.UpdateNowPlayingAsync(_currentTrack, (int)_remainingSeconds);
        }
        
        public void SkipToPercentage(double newPercentage)
        {
            _player.SkipToPercentage(newPercentage);

            _playingAt = DateTime.Now;
            _secondsPlayed = (newPercentage / 100) * _totalSeconds;
            UpdateRemainingSeconds();

            _scrobblingService.UpdateNowPlayingAsync(_currentTrack, (int)_remainingSeconds);
        }

        private void ProcessTrackChange(Track previousTrack)
        {
            if (previousTrack != null)
            {
                var previousTrackPlayedFor = _secondsPlayed + (DateTime.Now - _playingAt).TotalSeconds;

                if (_scrobblingRulesService.CanScrobble(_totalSeconds, previousTrackPlayedFor))
                {
                    _scrobblingService.ScrobbleAsync(previousTrack, DateTime.Now);
                }
            }

            if (_currentTrack != null)
            {
                _playingAt = DateTime.Now;
                _secondsPlayed = 0;

                UpdateRemainingSeconds();

                _scrobblingService.UpdateNowPlayingAsync(_currentTrack, (int)_remainingSeconds);
            }
            else
            {
                _scrobblingService.UpdateNowPlayingAsync(previousTrack, 30);
            }
        }

        private void UpdateCurrentTrack(Track track)
        {
            if (track == null)
            {
                _currentTrack = null;
                return;
            }

            _currentTrack = track;
        }
        
        private void UpdateRemainingSeconds()
        {
            _remainingSeconds = _totalSeconds - _secondsPlayed;
        }
    }
}
