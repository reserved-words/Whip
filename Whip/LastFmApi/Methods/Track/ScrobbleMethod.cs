using LastFmApi.Internal;
using System;

namespace LastFmApi.Methods.Track
{
    internal class ScrobbleMethod : ApiMethodBase
    {
        public ScrobbleMethod(AuthorizedApiClient client, LastFmApi.Track track, DateTime timePlayed)
            : base(client, "track.scrobble")
        {
            Parameters.Add(ParameterKey.Track, track.Title);
            Parameters.Add(ParameterKey.Artist, track.Artist);
            Parameters.Add(ParameterKey.Album, track.Album);
            Parameters.Add(ParameterKey.AlbumArtist, track.AlbumArtist);

            var scrobbleTime = timePlayed.ToUniversalTime();
            var unixTimeStamp = (scrobbleTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            Parameters.Add(ParameterKey.Timestamp, unixTimeStamp.ToString());

            AddApiSignature();
        }
    }
}
