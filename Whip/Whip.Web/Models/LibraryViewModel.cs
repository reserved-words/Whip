using System.Collections.Generic;

namespace Whip.Web.Models
{
    public class LibraryViewModel
    {
        public LibraryViewModel()
        {
            Artists = new List<ArtistViewModel>();
        }

        public List<ArtistViewModel> Artists { get; set; }
    }
}