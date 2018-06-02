using LastFmApi.Internal;
using System.Collections.Generic;

namespace LastFmApi.Methods.Track
{
    internal class UpdateNowPlayingMethod : ApiMethodBase
    {
        public UpdateNowPlayingMethod(UserApiClient client, LastFmApi.Track track, int duration)
            : base(client, "track.updateNowPlaying")
        {
            SetParameters(new Dictionary<ParameterKey, string>
            {
                { ParameterKey.Track, track.Title },
                { ParameterKey.Artist, track.Artist },
                { ParameterKey.Album, track.Album },
                { ParameterKey.AlbumArtist, track.AlbumArtist },
                { ParameterKey.Duration, duration.ToString() }
            });
        }
    }
}
