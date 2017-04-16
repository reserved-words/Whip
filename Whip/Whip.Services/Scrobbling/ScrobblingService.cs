using System;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class ScrobblingService : IScrobblingService
    {
        private const int MinimumTrackDuration = 31;

        private readonly IScrobblingService _scrobblingService;
        private readonly IUserSettings _userSettings;

        private Track _track;
        private int _duration;

        public ScrobblingService(IScrobblingService scrobblingService, IUserSettings userSettings)
        {
            _scrobblingService = scrobblingService;
            _userSettings = userSettings;

            _userSettings.ScrobblingStatusChanged += OnScrobblingStatusChanged;
        }

        private async void OnScrobblingStatusChanged()
        {
            if (_track == null)
                return;
            
            await UpdateNowPlayingAsync(_track, _userSettings.Scrobbling ? _duration : MinimumTrackDuration);
        }

        public async Task ScrobbleAsync(Track track, DateTime timePlayed)
        {
            if (!_userSettings.Scrobbling)
                return;

            // Catch exceptions and cache failed scrobbles to try again later
            await _scrobblingService.ScrobbleAsync(track, timePlayed);
        }

        public async Task UpdateNowPlayingAsync(Track track, int duration)
        {
            _track = track;
            _duration = duration;

            if (!_userSettings.Scrobbling)   
                return;

            await _scrobblingService.UpdateNowPlayingAsync(track, duration);
        }
    }
}
