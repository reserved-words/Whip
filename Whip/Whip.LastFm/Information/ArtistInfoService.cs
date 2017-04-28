using LastFmApi;
using LastFmApi.Interfaces;
using System;
using System.Threading.Tasks;
using Whip.Services.Interfaces;
using Whip.Common.Model;

namespace Whip.LastFm
{
    public class ArtistInfoService : IWebArtistInfoService
    {
        private readonly Lazy<ILastFmArtistInfoService> _lastFmService;
        private readonly IErrorHandlingService _errorHandler;

        public ArtistInfoService(ILastFmApiClientService clientService, IErrorHandlingService errorHandler)
        {
            _lastFmService = new Lazy<ILastFmArtistInfoService>(() => new LastFmArtistInfoService(clientService.ApiClient));
            _errorHandler = errorHandler;
        }

        public async Task<ArtistWebInfo> PopulateArtistImages(Artist artist)
        {
            if (!AreImagesPopulated(artist.WebInfo))
            {
                try
                {
                    var info = await _lastFmService.Value.GetInfo(artist.Name);

                    artist.WebInfo.SmallImageUrl = info.SmallImageUrl;
                    artist.WebInfo.MediumImageUrl = info.MediumImageUrl;
                    artist.WebInfo.LargeImageUrl = info.LargeImageUrl;
                    artist.WebInfo.ExtraLargeImageUrl = info.ExtraLargeImageUrl;
                }
                catch (LastFmApiException ex)
                {
                    _errorHandler.HandleError(ex, "Artist Name: " + artist.Name);
                }
            }

            return artist.WebInfo;
        }

        private bool AreImagesPopulated(ArtistWebInfo webInfo)
        {
            return !string.IsNullOrEmpty(webInfo.SmallImageUrl)
                || !string.IsNullOrEmpty(webInfo.MediumImageUrl)
                || !string.IsNullOrEmpty(webInfo.LargeImageUrl)
                || !string.IsNullOrEmpty(webInfo.ExtraLargeImageUrl);
        }
    }
}
