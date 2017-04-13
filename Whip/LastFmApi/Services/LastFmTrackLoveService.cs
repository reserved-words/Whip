using LastFmApi.Interfaces;
using LastFmApi.Internal;
using LastFmApi.Methods.Track;
using System;
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
            throw new NotImplementedException();
        }

        public async Task UnloveTrackAsync(Track track)
        {
            throw new NotImplementedException();
        }
    }
}
