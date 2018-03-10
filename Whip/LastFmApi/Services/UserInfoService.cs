using System.Collections.Generic;
using LastFmApi.Interfaces;
using LastFmApi.Internal;
using LastFmApi.Methods.Album;
using System.Threading.Tasks;
using LastFmApi.Methods.User;
using LastFmApi.Model;

namespace LastFmApi
{
    public class UserInfoService : IUserInfoService
    {
        public async Task<ICollection<TrackPlayInfo>> GetRecentTracks(ApiClient client, string username, int limit)
        {
            var getInfoMethod = new GetRecentTracksMethod(client, username, limit);
            return await getInfoMethod.GetResultAsync();
        }
    }
}
