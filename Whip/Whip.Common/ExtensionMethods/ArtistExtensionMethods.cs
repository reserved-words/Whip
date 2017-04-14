using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.Common.ExtensionMethods
{
    public static class ArtistExtensionMethods
    {
        public static List<Album> GetAlbumsInOrder(this Artist artist)
        {
            return artist.Albums
                .OrderBy(a => a.ReleaseType)
                .ThenBy(a => a.Year)
                .ToList();
        }

        public static List<Track> GetTracksInOrder(this Artist artist)
        {
            return artist.Tracks
                .OrderBy(t => t.Disc.Album.ReleaseType)
                .ThenBy(t => t.Disc.Album.Year)
                .ThenBy(t => t.Disc.DiscNo)
                .ThenBy(t => t.TrackNo)
                .ToList();
        }
    }
}
