using LastFmApi.Interfaces;
using System;
using System.Threading.Tasks;
using LastFmApi;

namespace Whip.LastFm
{
    public class AuthorizedTrackLoveService : ILastFmTrackLoveService
    {
        private readonly Lazy<ILastFmTrackLoveService> _trackLoveService;

        public AuthorizedTrackLoveService(ILastFmApiClientService clientService)
        {
            _trackLoveService = new Lazy<ILastFmTrackLoveService>(() => GetTrackLoveService(clientService));
        }
        
        public async Task LoveTrackAsync(Track track)
        {
            await _trackLoveService.Value.LoveTrackAsync(track);
        }

        public async Task UnloveTrackAsync(Track track)
        {
            await _trackLoveService.Value.UnloveTrackAsync(track);
        }

        public async Task<bool> IsLovedAsync(Track track)
        {
            return await _trackLoveService.Value.IsLovedAsync(track);
        }

        private ILastFmTrackLoveService GetTrackLoveService(ILastFmApiClientService clientService)
        {
            return new LastFmTrackLoveService(clientService.AuthorizedApiClient);
        }
    }
}
