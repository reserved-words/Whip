using LastFmApi.Interfaces;
using System;
using LastFmApi;
using System.Threading.Tasks;

namespace Whip.LastFm
{
    public class AuthorizedScrobblingService : ILastFmScrobblingService
    {
        private readonly Lazy<ILastFmScrobblingService> _scrobblingService;

        public AuthorizedScrobblingService(ILastFmApiClientService clientService)
        {
            _scrobblingService = new Lazy<ILastFmScrobblingService>(() => GetScrobblingService(clientService));
        }

        public async Task ScrobbleAsync(Track track, DateTime timePlayed)
        {
            await _scrobblingService.Value.ScrobbleAsync(track, timePlayed);
        }

        public async Task UpdateNowPlayingAsync(Track track, int duration)
        {
            await _scrobblingService.Value.UpdateNowPlayingAsync(track, duration);
        }

        private ILastFmScrobblingService GetScrobblingService(ILastFmApiClientService clientService)
        {
            return new LastFmScrobblingService(clientService.AuthorizedApiClient);
        }
    }
}
