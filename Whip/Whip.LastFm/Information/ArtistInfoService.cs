using LastFmApi.Interfaces;
using System.Threading.Tasks;
using Whip.Services.Interfaces;
using Whip.Common.Model;

namespace Whip.LastFm
{
    public class ArtistInfoService : IWebArtistInfoService
    {
        private readonly ILastFmApiClientService _clientService;
        private readonly IArtistInfoService _artistInfoService;

        public ArtistInfoService(ILastFmApiClientService clientService, IArtistInfoService artistInfoService)
        {
            _artistInfoService = artistInfoService;
            _clientService = clientService;
        }

        public async Task<ArtistWebInfo> PopulateArtistImages(Artist artist)
        {
            var info = await _artistInfoService.GetInfo(_clientService.ApiClient, artist.Name);

            artist.WebInfo.SmallImageUrl = info.SmallImageUrl;
            artist.WebInfo.MediumImageUrl = info.MediumImageUrl;
            artist.WebInfo.LargeImageUrl = info.LargeImageUrl;
            artist.WebInfo.ExtraLargeImageUrl = info.ExtraLargeImageUrl;

            return artist.WebInfo;
        }
    }
}
