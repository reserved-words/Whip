using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastFmApi
{
    public class Track
    {
        public Track(string title, string artistName, string albumTitle, string albumArtistName)
        {
            Title = title;
            Artist = artistName;
            Album = albumTitle;
            AlbumArtist = albumArtistName;
        }

        public string Title { get; private set; }
        public string Artist { get; private set; }
        public string Album { get; private set; }
        public string AlbumArtist { get; private set; }
    }
}
