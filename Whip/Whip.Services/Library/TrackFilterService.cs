using System.Collections.Generic;
using System.Linq;
using Whip.Common.ExtensionMethods;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class TrackFilterService : ITrackFilterService
    {
        private readonly Library _library;
        private readonly IDefaultTrackSorter _trackSorter;

        public TrackFilterService(Library library, IDefaultTrackSorter trackSorter)
        {
            _library = library;
            _trackSorter = trackSorter;
        }

        public List<Track> GetAll()
        {
            return _library.Artists
                .SelectMany(a => a.Tracks)
                .SortUsing(_trackSorter)
                .ToList();
        }

        public List<Track> GetTracksByGrouping(string grouping)
        {
            return _library.Artists
                .Where(a => a.Grouping == grouping)
                .SelectMany(a => a.Tracks)
                .SortUsing(_trackSorter)
                .ToList();
        }

        public List<Track> GetTracksByArtist(Artist artist)
        {
            return artist.Tracks
                .Union(artist.Albums.SelectMany(al => al.Discs).SelectMany(d => d.Tracks))
                .SortUsing(_trackSorter)
                .ToList();
        }

        public List<Track> GetAlbumTracksByArtist(Artist artist)
        {
            return artist.Albums
                .SelectMany(a => a.Discs)
                .SelectMany(d => d.Tracks)
                .SortUsing(_trackSorter)
                .ToList(); ;
        }

        public List<Track> GetTracksFromAlbum(Album album)
        {
            return album.Discs
                .SelectMany(d => d.Tracks)
                .SortUsing(_trackSorter)
                .ToList();
        }
    }
}
