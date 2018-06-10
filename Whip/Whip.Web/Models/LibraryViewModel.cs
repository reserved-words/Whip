using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.ExtensionMethods;
using Whip.Common.Model;

namespace Whip.Web.Models
{
    public class LibraryViewModel
    {
        public LibraryViewModel(IEnumerable<Artist> artists, Func<Artist, string> getPlayUrl)
        {
            Artists = artists
                .GroupBy(a => GetAlphabeticCategory(a.Sort))
                .ToDictionary(grp => grp.Key, grp => grp
                    .OrderBy(a => a.Sort)
                    .Select(a => new ArtistViewModel(a, getPlayUrl(a)))
                    .ToList());
        }

        public Dictionary<string, List<ArtistViewModel>> Artists { get; set; }

        private string GetAlphabeticCategory(string name)
        {
            return string.IsNullOrEmpty(name)
                ? ""
                : name.GetFirstWord().IsInteger()
                ? "#"
                : name.Substring(0, 1).ToUpperInvariant();
        }
    }
}