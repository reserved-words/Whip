using System.Collections.Generic;
using System.Linq;
using Whip.Common.ExtensionMethods;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Common.TrackSorters;
using Whip.Common;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class TrackFilterService : ITrackFilterService
    {
        private readonly Library _library;

        private static Dictionary<SortType, ITrackSorter> _sorters = new Dictionary<SortType, ITrackSorter>
        {
            { SortType.Random, new RandomTrackSorter() },
            { SortType.Ordered, new DefaultTrackSorter() }
        };

        public TrackFilterService(Library library)
        {
            _library = library;
        }

        public List<Track> GetAll(SortType sortType)
        {
            return _library.Artists
                .SelectMany(a => a.Tracks)
                .SortUsing(_sorters[sortType])
                .ToList();
        }

        public List<Track> GetTracksByGrouping(string grouping, SortType sortType)
        {
            return _library.Artists
                .Where(a => a.Grouping == grouping)
                .SelectMany(a => a.Tracks)
                .SortUsing(_sorters[sortType])
                .ToList();
        }

        public List<Track> GetTracksByArtists(List<Artist> artists, SortType sortType)
        {
            return artists
                .SelectMany(a => a.Tracks)
                .SortUsing(_sorters[sortType])
                .ToList();
        }

        public List<Track> GetAlbumTracksByArtists(List<Artist> artists, SortType sortType)
        {
            return artists
                .SelectMany(a => a.Albums)
                .SelectMany(a => a.Discs)
                .SelectMany(d => d.Tracks)
                .SortUsing(_sorters[sortType])
                .ToList();
        }

        public List<Track> GetTracksFromAlbums(List<Album> albums, SortType sortType)
        {
            return albums
                .SelectMany(a => a.Discs)
                .SelectMany(d => d.Tracks)
                .SortUsing(_sorters[sortType])
                .ToList();
        }
    }
}
