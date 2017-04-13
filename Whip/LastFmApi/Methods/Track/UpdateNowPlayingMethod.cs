using LastFmApi.Internal;

namespace LastFmApi.Methods.Track
{
    internal class UpdateNowPlayingMethod : ApiMethodBase
    {
        public UpdateNowPlayingMethod(AuthorizedApiClient client, LastFmApi.Track track, int duration)
            : base(client, "track.updateNowPlaying")
        {
            Parameters.Add(ParameterKey.Track, track.Title);
            Parameters.Add(ParameterKey.Artist, track.Artist);
            Parameters.Add(ParameterKey.Album, track.Album);
            Parameters.Add(ParameterKey.AlbumArtist, track.AlbumArtist);
            Parameters.Add(ParameterKey.Duration, duration.ToString());

            AddApiSignature();
        }
    }
}
