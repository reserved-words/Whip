using LastFmApi.Interfaces;
using System;
using System.Threading.Tasks;
using LastFmApi;

namespace Whip.LastFm
{
    public class AuthorizedTrackLoveService : ILastFmTrackLoveService
    {
        private readonly Lazy<Task<ILastFmTrackLoveService>> _trackLoveService;

        public AuthorizedTrackLoveService(ILastFmApiClientService clientService)
        {
            _trackLoveService = new Lazy<Task<ILastFmTrackLoveService>>(() => GetTrackLoveService(clientService));
        }
        
        public async Task LoveTrackAsync(Track track)
        {
            var service = await _trackLoveService.Value;
            await service.LoveTrackAsync(track);
        }

        public async Task UnloveTrackAsync(Track track)
        {
            var service = await _trackLoveService.Value;
            await service.UnloveTrackAsync(track);
        }

        public async Task<bool> IsLovedAsync(Track track)
        {
            var service = await _trackLoveService.Value;
            return await service.IsLovedAsync(track);
        }

        private async Task<ILastFmTrackLoveService> GetTrackLoveService(ILastFmApiClientService clientService)
        {
            return new LastFmTrackLoveService(await clientService.AuthorizedApiClient);
        }
    }
}
