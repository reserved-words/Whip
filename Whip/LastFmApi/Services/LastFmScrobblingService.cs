using LastFmApi.Interfaces;
using LastFmApi.Internal;
using LastFmApi.Methods.Track;
using System;
using System.Threading.Tasks;

namespace LastFmApi
{
    public class LastFmScrobblingService : ILastFmScrobblingService
    {
        private readonly AuthorizedApiClient _client;

        public LastFmScrobblingService(AuthorizedApiClient client)
        {
            _client = client;
        }

        public async Task ScrobbleAsync(Track track, DateTime timePlayed)
        {
            var scrobbleMethod = new ScrobbleMethod(_client, track, timePlayed);
            await scrobbleMethod.PostAsync();
        }

        public async Task UpdateNowPlayingAsync(Track track, int duration)
        {
            var updateNowPlayingMethod = new UpdateNowPlayingMethod(_client, track, duration);
            await updateNowPlayingMethod.PostAsync();
        }
    }
}
