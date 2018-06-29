using Whip.Common.ExtensionMethods;
using Whip.Common.Model;

namespace Whip.Web.ExtensionMethods
{
    public static class TrackExtensionMethods
    {
        public static bool IsByAlbumArtist(this Track track)
        {
            return track.Artist.Name == track.Disc.Album.Artist.Name;
        }
    }
}