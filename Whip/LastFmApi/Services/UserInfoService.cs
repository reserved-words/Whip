using System.Collections.Generic;
using LastFmApi.Interfaces;
using LastFmApi.Internal;
using System.Threading.Tasks;
using LastFmApi.Methods.User;
using LastFmApi.Model;

namespace LastFmApi
{
    public class UserInfoService : IUserInfoService
    {
        public async Task<ICollection<TrackPlayInfo>> GetRecentTracks(ApiClient client, string username, int limit)
        {
            var method = new GetRecentTracksMethod(client, username, limit);
            return await method.GetResultAsync();
        }

        public async Task<ICollection<UserArtistInfo>> GetTopArtists(ApiClient client, string username, TimePeriod period, int limit)
        {
            var method = new GetTopArtistsMethod(client, username, period, limit);
            return await method.GetResultAsync();
        }

        public async Task<ICollection<UserAlbumInfo>> GetTopAlbums(ApiClient client, string username, TimePeriod period, int limit)
        {
            var method = new GetTopAlbumsMethod(client, username, period, limit);
            return await method.GetResultAsync();
        }

        public async Task<ICollection<UserTrackInfo>> GetTopTracks(ApiClient client, string username, TimePeriod period, int limit)
        {
            var method = new GetTopTracksMethod(client, username, period, limit);
            return await method.GetResultAsync();
        }
    }
}
