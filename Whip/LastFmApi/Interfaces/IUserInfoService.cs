using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LastFmApi.Model;

namespace LastFmApi.Interfaces
{
    public interface IUserInfoService
    {
        Task<ICollection<TrackPlayInfo>> GetRecentTracks(ApiClient client, string username, int limit);
        Task<ICollection<UserArtistInfo>> GetTopArtists(ApiClient client, string username, TimePeriod period, int limit);
        Task<ICollection<UserAlbumInfo>> GetTopAlbums(ApiClient client, string username, TimePeriod period, int limit);
        Task<ICollection<UserTrackInfo>> GetTopTracks(ApiClient client, string username, TimePeriod period, int limit);
    }
}
