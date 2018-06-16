using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Whip.Web.Models
{
    public class LibraryArtistListViewModel
    {
        public LibraryArtistListViewModel(string category, List<ArtistViewModel> artists)
        {
            Category = category;
            Artists = artists;
        }

        public string Category { get; }
        public List<ArtistViewModel> Artists { get; }
    }
}