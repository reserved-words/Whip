using LastFmApi.Internal;
using System;
using System.Collections.Generic;

namespace LastFmApi.Methods.Track
{
    internal class ScrobbleMethod : ApiMethodBase
    {
        public ScrobbleMethod(UserApiClient client, LastFmApi.Track track, DateTime timePlayed)
            : base(client, "track.scrobble")
        {
            var scrobbleTime = timePlayed.ToUniversalTime();
            var unixTimeStamp = (scrobbleTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            
            SetParameters(new Dictionary<ParameterKey, string>
            {
                { ParameterKey.Track, track.Title },
                { ParameterKey.Artist, track.Artist },
                { ParameterKey.Album, track.Album },
                { ParameterKey.AlbumArtist, track.AlbumArtist },
                { ParameterKey.Timestamp, unixTimeStamp.ToString() }
            });
        }
    }
}
