using LastFmApi.Internal;
using System.Collections.Generic;

namespace LastFmApi.Methods.Track
{
    internal class LoveTrackMethod : ApiMethodBase
    {
        public LoveTrackMethod(AuthorizedApiClient client, LastFmApi.Track track)
            : base(client, "track.love")
        {
            SetParameters(new Dictionary<ParameterKey, string>
            {
                { ParameterKey.Track, track.Title },
                { ParameterKey.Artist, track.Artist }
            });
        }
    }
}
