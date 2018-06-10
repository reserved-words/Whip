using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.Model;

namespace Whip.Web.Models
{
    public class LibraryAlbumViewModel
    {
        private readonly Album _album;

        public LibraryAlbumViewModel(Album album, string artworkUrl)
        {
            _album = album;
            ArtworkUrl = artworkUrl;
            Discs = album.Discs.Select(d => new LibraryDiscViewModel(d)).ToList();
        }

        public string Title => _album.Title;
        public string Artist => _album.Artist.Name;
        public string Year => _album.Year;

        public string ArtworkUrl { get; }

        public List<LibraryDiscViewModel> Discs { get; set; }
    }
}