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
            return artists.OrderBy(a => a.Sort);
        }

        public void SortDiscs(Album album)
        {
            album.Discs = album.Discs.OrderBy(d => d.DiscNo).ToList();
        }

        public void SortTracks(Disc disc)
        {
            disc.Tracks = disc.Tracks.OrderBy(t => t.TrackNo).ToList();
        }

        public string SortValue(Album album)
        {
            return string.Format("{0} {1}", album.ReleaseType.GetDisplayName(), album.Year);
        }

        public IEnumerable<Track> GetArtistTracksInDefaultOrder(Artist artist)
        {
           return DefaultSort(artist.Tracks);
        }

        public IEnumerable<Track> GetAlbumTracksInDefaultOrder(Artist artist)
        {
            return DefaultSort(artist.Albums
                .SelectMany(a => a.Discs)
                .SelectMany(d => d.Tracks));
        }

        private static IEnumerable<Track> DefaultSort(IEnumerable<Track> tracks)
        {
            return tracks.OrderBy(t => t.Disc.Album.Artist.SortName)
                .ThenBy(t => t.Disc.Album.ReleaseType)
                .ThenBy(t => t.Disc.Album.Year)
                .ThenBy(t => t.Disc.Album.Title)
                .ThenBy(t => t.Disc.DiscNo)
                .ThenBy(t => t.TrackNo);
        }
    }
}
