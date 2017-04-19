using LastFmApi;
using LastFmApi.Interfaces;
using System;
using System.Threading.Tasks;
using Whip.Services.Interfaces;

namespace Whip.LastFm
{
    public class AlbumInfoService : IWebAlbumInfoService
    {
        private readonly Lazy<ILastFmAlbumInfoService> _lastFmService;

        public AlbumInfoService(ILastFmApiClientService clientService)
        {
            _lastFmService = new Lazy<ILastFmAlbumInfoService>(() => new LastFmAlbumInfoService(clientService.ApiClient));
        }

        public async Task<string> GetArtworkUrl(string artistName, string albumTitle)
        {
            return await _lastFmService.Value.GetArtworkUrl(artistName, albumTitle);
        }
    }
}
