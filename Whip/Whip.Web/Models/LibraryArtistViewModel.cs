using System.Collections.Generic;

namespace Whip.Web.Models
{
    public class LibraryArtistViewModel
    {
        public LibraryArtistViewModel()
        {
            Albums = new List<LibraryAlbumViewModel>();
        }

        public string Name { get; set; }
        public List<LibraryAlbumViewModel> Albums { get; set; }
    }
}