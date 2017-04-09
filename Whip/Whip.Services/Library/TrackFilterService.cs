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

        public TrackFilterService(Library library)
        {
            _library = library;
        }

        public List<Track> GetAll(ITrackSorter sorter, string grouping = null)
        {
            return _library.Artists
                .Where(a => (grouping == null || a.Grouping == grouping))
                .SelectMany(a => a.Tracks)
                .SortUsing(sorter)
                .ToList();
        }
    }
}
