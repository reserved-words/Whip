using System.Collections.Generic;
using Whip.Common.Interfaces;
using Whip.Common.Model;

namespace Whip.Common.ExtensionMethods
{
    public static class TrackSortExtensionMethods
    {
        public static IEnumerable<Track> SortUsing(this IEnumerable<Track> tracks, ITrackSorter sorter)
        {
            return sorter.Sort(tracks);
        }
    }
}
