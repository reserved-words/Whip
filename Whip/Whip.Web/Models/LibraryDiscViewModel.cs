using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.Model;

namespace Whip.Web.Models
{
    public class LibraryDiscViewModel
    {
        private readonly Disc _disc;

        public LibraryDiscViewModel(Disc disc, Func<Track, string> getTrackUrl, Func<Album, string> getArtWorkUrl)
        {
            _disc = disc;
            Tracks = disc.Tracks
                .OrderBy(t => t.TrackNo)
                .Select(t => new TrackViewModel(t, getTrackUrl(t), getArtWorkUrl(t.Disc.Album))).ToList();
        }

        public string Title => _disc.Album.Discs.Count() == 1
            ? ""
            : $"Disc {_disc.DiscNo}";

        public List<TrackViewModel> Tracks { get; set; }
    }
}