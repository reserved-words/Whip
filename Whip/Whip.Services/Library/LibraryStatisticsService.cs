using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class LibraryStatisticsService :ILibraryStatisticsService
    {
        private readonly Library _library;

        public LibraryStatisticsService(Library library)
        {
            _library = library;
        }

        public int GetNumberOfTrackArtists()
        {
            return _library.Artists.Count;
        }

        public int GetNumberOfAlbumArtists()
        {
            return _library.Artists.Count(a => a.Albums.Any());
        }

        public int GetNumberOfAlbums()
        {
            return _library.Artists.Sum(a => a.Albums.Count);
        }

        public int GetNumberOfTracks()
        {
            return _library.Artists.Sum(a => a.Tracks.Count);
        }

        public TimeSpan GetTotalTime()
        {
            return TimeSpan.FromSeconds(_library.Artists
                .SelectMany(a => a.Tracks)
                .Sum(t => t.Duration.TotalSeconds));
        }

        public ICollection<Tuple<ReleaseType, int>> GetAlbumsByReleaseType()
        {
            return _library.Artists
                .SelectMany(a => a.Albums)
                .GroupBy(a => a.ReleaseType)
                .OrderBy(g => g.Key)
                .Select(g => new Tuple<ReleaseType, int>(g.Key, g.Count()))
                .ToList();
        }

        public ICollection<Tuple<string, int>> GetArtistsByGrouping()
        {
            return _library.Artists
                .GroupBy(a => a.Grouping)
                .OrderBy(g => g.Key)
                .Select(g => new Tuple<string, int>(string.IsNullOrEmpty(g.Key) ? "(None)" : g.Key, g.Count()))
                .ToList();
        }

        public ICollection<Tuple<string, int>> GetTracksByArtist(int max)
        {
            return _library.Artists
                .OrderByDescending(a => a.Tracks.Count)
                .Take(max)
                .Select(a => new Tuple<string, int>(a.Name, a.Tracks.Count))
                .ToList();
        }
    }
}
