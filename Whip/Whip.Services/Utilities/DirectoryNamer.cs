using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class DirectoryNamer : IDirectoryNamer
    {
        public string GetArtistDirectoryName(Track track)
        {
            return ReplaceSpecialCharacters(track.Disc.Album.Artist.Name);
        }

        public string GetAlbumDirectoryName(Track track)
        {
            return ReplaceSpecialCharacters(track.Disc.Album.Title);
        }

        private static string ReplaceSpecialCharacters(string str)
        {
            return str.Replace(":", "_");
        }
    }
}
