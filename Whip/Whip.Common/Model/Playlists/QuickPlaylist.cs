using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.Enums;
using Whip.Common.ExtensionMethods;

namespace Whip.Common.Model
{
    public class QuickPlaylist : Playlist
    {
        public QuickPlaylist(int id, string title, bool favourite, FilterType filterType, params string[] filterValues)
            :base(PlaylistType.Quick, id, title, favourite)
        {
            FilterType = filterType;
            FilterValues = filterValues;
        }

        public FilterType FilterType { get; set; }
        public string[] FilterValues { get; set; }

        public string GetDefaultTitle()
        {
            return $"{FilterType.GetDisplayName()}: {GetFilterDescription()}";
        }

        private string GetFilterDescription()
        {
            return FilterType == FilterType.DateAdded
                ? $"In the last {FilterValues[0]} days"
                : string.Join(", ", FilterValues.Where(v => !string.IsNullOrEmpty(v)));
        }
    }
}
