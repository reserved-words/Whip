using LastFmApi.Interfaces;
using LastFmApi.Internal;
using LastFmApi.Methods.Track;
using System;
using System.Threading.Tasks;

namespace LastFmApi
{
    public class ScrobblingService : IScrobblingService
    {
        public async Task ScrobbleAsync(UserApiClient client, Track track, DateTime timePlayed)
        {
            var scrobbleMethod = new ScrobbleMethod(client, track, timePlayed);
            await scrobbleMethod.PostAsync();
        }

        public async Task UpdateNowPlayingAsync(UserApiClient client, Track track, int duration)
        {
            var updateNowPlayingMethod = new UpdateNowPlayingMethod(client, track, duration);
            await updateNowPlayingMethod.PostAsync();
        }
    }
}
