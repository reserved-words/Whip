using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface ILibraryStatisticsService
    {
        int GetNumberOfTrackArtists();
        int GetNumberOfAlbumArtists();
        int GetNumberOfAlbums();
        int GetNumberOfTracks();
        TimeSpan GetTotalTime();
        ICollection<Tuple<ReleaseType, int>> GetAlbumsByReleaseType();
        ICollection<Tuple<string, int>> GetArtistsByGrouping();
        ICollection<Tuple<string, int>> GetTracksByArtist(int max);
    }
}
