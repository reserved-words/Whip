using LastFmApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastFmApi
{
    public class ScrobblingService : IScrobblingService
    {
        public void Scrobble(Track track, DateTime timePlayed)
        {
            
        }

        public string StartSession(string apiKey, string secret, string user, string sessionKey = null)
        {
            return string.Empty;
        }

        public void UpdateNowPlaying(Track track, double duration)
        {
            
        }
    }
}
