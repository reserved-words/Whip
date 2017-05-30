using LastFmApi.Interfaces;
using System.Threading.Tasks;
using Whip.Services.Interfaces;
using Whip.Common.Model;
using System.Linq;
using System;

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

        public async Task<ArtistWebInfo> PopulateArtistInfo(Artist artist, int numberOfSimilarArtists)
        {
            var info = await _artistInfoService.GetInfo(_clientService.ApiClient, artist.Name);

            artist.WebInfo.Updated = DateTime.Now;
            artist.WebInfo.Wiki = info.Wiki;
            artist.WebInfo.SmallImageUrl = info.SmallImageUrl;
            artist.WebInfo.MediumImageUrl = info.MediumImageUrl;
            artist.WebInfo.LargeImageUrl = info.LargeImageUrl;
            artist.WebInfo.ExtraLargeImageUrl = info.ExtraLargeImageUrl;

            artist.WebInfo.SimilarArtists = info.SimilarArtists
                .Take(numberOfSimilarArtists)
                .Select(sa => new ArtistWebSimilarArtist
                {
                    Name = sa.Name,
                    Url = sa.Url,
                    ImageUrl = sa.ExtraLargeImageUrl
                })
                .ToList();

            return artist.WebInfo;
        }
    }
}
