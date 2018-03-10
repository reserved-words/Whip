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
    }
}
