using System.Collections.Generic;
using System.Linq;
using Whip.Common.Model;

namespace Whip.Web.Models
{
    public class LibraryDiscViewModel
    {
        private readonly Disc _disc;

        public LibraryDiscViewModel(Disc disc)
        {
            _disc = disc;
            Tracks = disc.Tracks.Select(t => new LibraryTrackViewModel(t)).ToList();
        }

        public string Title => _disc.Album.DiscCount == 1
            ? ""
            : $"Disc {_disc.DiscNo}";

        public List<LibraryTrackViewModel> Tracks { get; set; }
    }
}