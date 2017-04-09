using LastFmApi.Interfaces;
using System;
using Whip.Common.Interfaces;
using Whip.Services.Interfaces;
using Track = Whip.Common.Model.Track;
using LastFmTrack = LastFmApi.Track;
using LastFmApi;

namespace Whip.LastFm
{
    public class ScrobblingPlayer : IPlayer
    {
        private readonly IPlayer _player;
        private readonly IScrobblingRulesService _scrobblingRulesService;
        private readonly IScrobblingService _scrobblingService;
        private readonly IUserSettingsService _userSettingsService;

        private LastFmTrack _currentTrack;

        private double _totalSeconds;
        private double _secondsPlayed;
        private double _remainingSeconds;

        private DateTime _pausedAt;
        private DateTime _playingAt;

        private bool _scrobblingServiceStarted;

        public ScrobblingPlayer(IPlayer player, IScrobblingRulesService scrobblingRulesService, IUserSettingsService userSettingsService)
        {
            _player = player;
            _scrobblingRulesService = scrobblingRulesService;
            _userSettingsService = userSettingsService;

            _scrobblingService = new ScrobblingService();
        }

        public void Pause()
        {
            _pausedAt = DateTime.Now;
            _secondsPlayed = _secondsPlayed + (_pausedAt - _playingAt).TotalSeconds;
            UpdateRemainingSeconds();

            _player.Pause();

            ScrobblingService.UpdateNowPlaying(_currentTrack, 30);
        }

        public void Play(Track track)
        {
            var previousTrack = _currentTrack;
            _totalSeconds = track?.Duration.TotalSeconds ?? 0;
            _currentTrack = CreateLastFmTrack(track);

            _player.Play(track);

            ProcessTrackChange(previousTrack);
        }
        
        public void Resume()
        {
            _playingAt = DateTime.Now;

            _player.Resume();

            ScrobblingService.UpdateNowPlaying(_currentTrack, _remainingSeconds);
        }
        
        public void SkipToPercentage(double newPercentage)
        {
            _player.SkipToPercentage(newPercentage);

            _playingAt = DateTime.Now;
            _secondsPlayed = (newPercentage / 100) * _totalSeconds;
            UpdateRemainingSeconds();

            ScrobblingService.UpdateNowPlaying(_currentTrack, _remainingSeconds);
        }

        private void ProcessTrackChange(LastFmTrack previousTrack)
        {
            if (previousTrack != null)
            {
                var previousTrackPlayedFor = _secondsPlayed + (DateTime.Now - _playingAt).TotalSeconds;

                ScrobblingService.UpdateNowPlaying(previousTrack, 0);

                if (_scrobblingRulesService.CanScrobble(_totalSeconds, previousTrackPlayedFor))
                {
                    ScrobblingService.Scrobble(previousTrack, DateTime.Now);
                }
            }

            if (_currentTrack != null)
            {
                _playingAt = DateTime.Now;
                _secondsPlayed = 0;
                UpdateRemainingSeconds();

                ScrobblingService.UpdateNowPlaying(_currentTrack, _remainingSeconds);
            }
        }

        private IScrobblingService ScrobblingService
        {
            get
            {
                if (!_scrobblingServiceStarted)
                {
                    var sessionKey = _scrobblingService.StartSession(
                        _userSettingsService.LastFmApiKey,
                        _userSettingsService.LastFmApiSecret,
                        _userSettingsService.LastFmUsername,
                        _userSettingsService.LastFmApiSessionKey);

                    _userSettingsService.LastFmApiSessionKey = sessionKey;
                    _userSettingsService.Save();

                    _scrobblingServiceStarted = true;
                }

                return _scrobblingService;
            }
        }

        private LastFmTrack CreateLastFmTrack(Track track)
        {
            if (track == null)
                return null;

            return new LastFmTrack(track.Title, track.Artist.Name, track.Disc.Album.Title, track.Disc.Album.Artist.Name);
        }
        
        private void UpdateRemainingSeconds()
        {
            _remainingSeconds = _totalSeconds - _secondsPlayed;
        }
    }
}
