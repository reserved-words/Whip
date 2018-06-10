using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.Model;

namespace Whip.Web.Models
{
    public class LibraryArtistViewModel
    {
        public LibraryArtistViewModel(Artist artist, Func<Album, string> getArtworkUrl)
        {
            Artist = artist;
            Albums = artist.Albums.Select(a => new LibraryAlbumViewModel(a, getArtworkUrl(a))).ToList();
        }

        public Artist Artist { get; set; }

        public string Name => Artist.Name;
        public string Origin => Artist.City.Description;

        public List<LibraryAlbumViewModel> Albums { get; set; }
    }
}