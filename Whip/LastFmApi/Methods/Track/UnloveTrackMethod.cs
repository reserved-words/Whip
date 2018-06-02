using LastFmApi.Internal;
using System.Collections.Generic;

namespace LastFmApi.Methods.Track
{
    internal class UnloveTrackMethod : ApiMethodBase
    {
        public UnloveTrackMethod(UserApiClient client, LastFmApi.Track track)
            : base(client, "track.unlove")
        {
            SetParameters(new Dictionary<ParameterKey, string>
            {
                { ParameterKey.Track, track.Title },
                { ParameterKey.Artist, track.Artist }
            });
        }
    }
}
