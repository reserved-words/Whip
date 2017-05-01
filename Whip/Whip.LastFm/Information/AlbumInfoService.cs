using LastFmApi;
using LastFmApi.Interfaces;
using System;
using System.Threading.Tasks;
using Whip.Services.Interfaces;

namespace Whip.LastFm
{
    public class AlbumInfoService : IWebAlbumInfoService
    {
        private readonly ILastFmApiClientService _clientService;
        private readonly IAlbumInfoService _albumInfoService;

        public AlbumInfoService(ILastFmApiClientService clientService, IAlbumInfoService albumInfoService)
        {
            _albumInfoService = albumInfoService;
            _clientService = clientService;
        }

        public async Task<string> GetArtworkUrl(string artistName, string albumTitle)
        {
            return await _albumInfoService.GetArtworkUrl(_clientService.ApiClient, artistName, albumTitle);
        }
    }
}
