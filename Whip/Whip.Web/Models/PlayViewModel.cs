using System.Collections.Generic;

namespace Whip.Web.Models
{
    public class PlayViewModel
    {
        public PlayViewModel(string title, TrackListViewModel trackList, string playUrl = null)
        {
            Title = title;
            Tracks = trackList;
            PlayUrl = playUrl;
        }

        public string Title { get; }
        public TrackListViewModel Tracks { get; }
        public string PlayUrl { get; }
    }
}