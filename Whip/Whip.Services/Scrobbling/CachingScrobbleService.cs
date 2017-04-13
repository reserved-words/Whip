using System;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class CachingScrobbleService : IScrobblingService
    {
        private readonly IScrobblingService _service;

        public CachingScrobbleService(IScrobblingService service)
        {
            _service = service;
        }

        public async Task ScrobbleAsync(Track track, DateTime timePlayed)
        {
            // Catch exceptions and cache failed scrobbles to try again later
            await _service.ScrobbleAsync(track, timePlayed);
        }

        public async Task UpdateNowPlayingAsync(Track track, int duration)
        {
            await _service.UpdateNowPlayingAsync(track, duration);
        }
    }
}
