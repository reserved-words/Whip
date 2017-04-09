using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastFmApi.Interfaces
{
    public interface IScrobblingService
    {
        string StartSession(string apiKey, string secret, string user, string sessionKey);
        void Scrobble(Track track, DateTime timePlayed);
        void UpdateNowPlaying(Track track, double duration);
    }
}
