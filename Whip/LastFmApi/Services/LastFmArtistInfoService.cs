using LastFmApi.Interfaces;
using LastFmApi.Internal;
using LastFmApi.Methods.Artist;
using System.Threading.Tasks;

namespace LastFmApi
{
    public class LastFmArtistInfoService : ILastFmArtistInfoService
    {
        private readonly ApiClient _client;

        public LastFmArtistInfoService(ApiClient client)
        {
            _client = client;
        }

        public async Task<ArtistInfo> GetInfo(string artistName)
        {
            var getInfoMethod = new GetInfoMethod(_client, artistName);
            return await getInfoMethod.GetResultAsync();
        }
    }
}
