using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.ExtensionMethods;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class LibrarySortingService : ILibrarySortingService
    {
        public IOrderedEnumerable<Artist> GetInDefaultOrder(IEnumerable<Artist> artists)
        {
            return artists.OrderBy(a => SortValue(a));
        }

        public void SortDiscs(Album album)
        {
            album.Discs = album.Discs.OrderBy(d => d.DiscNo).ToList();
        }

        public void SortTracks(Disc disc)
        {
            disc.Tracks = disc.Tracks.OrderBy(t => t.TrackNo).ToList();
        }

        public string SortValue(Artist artist)
        {
            return artist.Name.Replace("The ", "");
        }

        public string SortValue(Album album)
        {
            return string.Format("{0} {1}", album.ReleaseType.GetDisplayName(), album.Year);
        }
    }
}
