using System;
using System.Collections.Generic;
using Whip.Common.Model;

namespace Whip.Web.Models
{
    public class LibraryTrackViewModel
    {
        private readonly Track _track;

        public LibraryTrackViewModel(Track track)
        {
            _track = track;
        }

        public string TrackNo => string.Format("{0:00}", _track.TrackNo);
        public string Title => _track.Title;
        public string Artist => _track.Artist.Name;
        public string Duration => _track.Duration.ToString(@"mm\:ss");
    }
}