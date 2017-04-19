using LastFmApi.Interfaces;
using LastFmApi.Internal;
using LastFmApi.Methods.Album;
using System.Threading.Tasks;

namespace LastFmApi
{
    public class LastFmAlbumInfoService : ILastFmAlbumInfoService
    {
        private readonly ApiClient _client;

        public LastFmAlbumInfoService(ApiClient client)
        {
            _client = client;
        }

        public async Task<string> GetArtworkUrl(string artistName, string albumTitle)
        {
            var getInfoMethod = new GetInfoMethod(_client, artistName, albumTitle);
            var albumInfo = await getInfoMethod.GetResultAsync();
            return albumInfo.ArtworkUrl;
        }
    }
}
