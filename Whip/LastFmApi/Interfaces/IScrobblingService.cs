using LastFmApi.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastFmApi.Interfaces
{
    public interface IScrobblingService
    {
        Task ScrobbleAsync(AuthorizedApiClient client, Track track, DateTime timePlayed);
        Task UpdateNowPlayingAsync(AuthorizedApiClient client, Track track, int duration);
    }
}
