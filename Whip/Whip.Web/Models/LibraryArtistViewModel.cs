using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.Model;
using Whip.Web.ExtensionMethods;

namespace Whip.Web.Models
{
    public class LibraryArtistViewModel
    {
        public LibraryArtistViewModel(Artist artist, string playUrl, Func<Album, string> getAlbumPlayUrl, Func<Album, string> getArtworkUrl)
        {
            Artist = artist;
            PlayUrl = playUrl;
            Albums = artist.Albums
                .OrderBy(a => a.ReleaseType)
                .ThenBy(a => a.Year)
                .Select(a => new LibraryAlbumViewModel(a, getAlbumPlayUrl(a), getArtworkUrl(a)))
                .ToList();
        }

        public Artist Artist { get; }
        public string PlayUrl { get; }

        public string Category => Artist.Category();
        public string Name => Artist.Name;
        public string Origin => Artist.City.Description;

        public List<LibraryAlbumViewModel> Albums { get; }
    }
}