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
    }
}
