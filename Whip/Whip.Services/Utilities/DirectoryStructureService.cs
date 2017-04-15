using System.IO;
using System.Linq;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class DirectoryStructureService : IDirectoryStructureService
    {
        public string GetArtworkPath(Album album)
        {
            var trackPath = album.Discs.FirstOrDefault()?
                .Tracks.FirstOrDefault()?
                .File.FullPath;

            var directory = Path.GetDirectoryName(trackPath);

            return Path.Combine(directory, "artwork.jpg");
        }
    }
}
