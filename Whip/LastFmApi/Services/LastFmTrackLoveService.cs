using LastFmApi.Interfaces;
using LastFmApi.Internal;
using LastFmApi.Methods.Track;
using System.Threading.Tasks;

namespace LastFmApi
{
    public class LastFmTrackLoveService : ILastFmTrackLoveService
    {
        private readonly AuthorizedApiClient _client;

        public LastFmTrackLoveService(AuthorizedApiClient apiClient)
        {
            _client = apiClient;
        }

        public async Task<bool> IsLovedAsync(Track track)
        {
            var isLovedMethod = new IsLovedMethod(_client, track, _client.Username);
            return await isLovedMethod.GetResultAsync();
        }

        public async Task LoveTrackAsync(Track track)
        {
            var loveTrackMethod = new LoveTrackMethod(_client, track);
            await loveTrackMethod.PostAsync();
        }

        public async Task UnloveTrackAsync(Track track)
        {
            var unloveTrackMethod = new UnloveTrackMethod(_client, track);
            await unloveTrackMethod.PostAsync();
        }
    }
}
