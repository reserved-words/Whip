using LastFmApi.Interfaces;
using System;
using LastFmApi;
using System.Threading.Tasks;

namespace Whip.LastFm
{
    public class AuthorizedScrobblingService : ILastFmScrobblingService
    {
        private readonly Lazy<Task<ILastFmScrobblingService>> _scrobblingService;

        private readonly ILastFmScrobblingService _service;

        public AuthorizedScrobblingService(ILastFmApiClientService clientService)
        {
            _scrobblingService = new Lazy<Task<ILastFmScrobblingService>>(() => GetScrobblingService(clientService));
        }

        public async Task ScrobbleAsync(Track track, DateTime timePlayed)
        {
            var scrobblingService = await _scrobblingService.Value;
            await scrobblingService.ScrobbleAsync(track, timePlayed);
        }

        public async Task UpdateNowPlayingAsync(Track track, int duration)
        {
            var scrobblingService = await _scrobblingService.Value;
            await scrobblingService.UpdateNowPlayingAsync(track, duration);
        }

        private async Task<ILastFmScrobblingService> GetScrobblingService(ILastFmApiClientService clientService)
        {
            return new LastFmScrobblingService(await clientService.AuthorizedApiClient);
        }
    }
}
