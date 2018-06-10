using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.Model;

namespace Whip.Web.Models
{
    public class LibraryArtistViewModel
    {
        public LibraryArtistViewModel(Artist artist, Func<Album, string> getAlbumPlayUrl, Func<Album, string> getArtworkUrl)
        {
            Artist = artist;
            Albums = artist.Albums
                .OrderBy(a => a.ReleaseType)
                .ThenBy(a => a.Year)
                .Select(a => new LibraryAlbumViewModel(a, getAlbumPlayUrl(a), getArtworkUrl(a)))
                .ToList();
        }

        public Artist Artist { get; set; }

        public string Name => Artist.Name;
        public string Origin => Artist.City.Description;

        public List<LibraryAlbumViewModel> Albums { get; set; }
    }
}