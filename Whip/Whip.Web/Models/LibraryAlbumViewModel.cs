using System.Collections.Generic;

namespace Whip.Web.Models
{
    public class LibraryAlbumViewModel
    {
        public string Title { get; set; }
        public string ArtworkUrl { get; set; }
        public List<string> Tracks { get; set; }
    }
}