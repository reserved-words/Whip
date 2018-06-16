using System;
using System.Collections.Generic;
using System.Linq;

namespace Whip.Web.Models
{
    public class LibraryViewModel
    {
        public LibraryViewModel()
        {
            Artists = GetDictionary();
        }

        public Dictionary<string, List<ArtistViewModel>> Artists { get; set; }

        private static Dictionary<string, List<ArtistViewModel>> GetDictionary()
        {
            var alphabet = "#ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            return alphabet.ToDictionary(a => a.ToString(), a => new List<ArtistViewModel>());
        }
    }
}