using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common;
using Whip.Common.ExtensionMethods;
using Whip.Common.Model;
using Whip.Web.ExtensionMethods;

namespace Whip.Web.Models
{
    public class LibraryArtistViewModel
    {
        public LibraryArtistViewModel(Artist artist, string playUrl, Func<Album, string> getAlbumPlayUrl, Func<Album, string> getAlbumInfoUrl)
        {
            Artist = artist;
            PlayUrl = playUrl;
            Albums = artist.Albums
                .GroupBy(a => a.ReleaseType.GetReleaseTypeGrouping())
                .OrderBy(grp => grp.Key)
                .ToDictionary(grp => grp.Key.GetDisplayName(), grp => grp
                    .OrderByDescending(a => a.Year)
                    .Select(a => new AlbumViewModel(a, getAlbumPlayUrl(a), getAlbumInfoUrl(a)))
                    .ToList());
        }

        public Artist Artist { get; }
        public string PlayUrl { get; }

        public string Category => Artist.Category();
        public string Name => Artist.Name;
        public string Origin => Artist.City.Description;

        public Dictionary<string, List<AlbumViewModel>> Albums { get; }
    }
}