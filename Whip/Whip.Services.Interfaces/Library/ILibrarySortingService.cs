using System.Collections.Generic;
using System.Linq;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface ILibrarySortingService
    {
        IOrderedEnumerable<Artist> GetInDefaultOrder(IEnumerable<Artist> artists);

        void SortDiscs(Album album);

        void SortTracks(Disc disc);

        string SortValue(Album album);

        IEnumerable<Track> GetArtistTracksInDefaultOrder(Artist artist);

        IEnumerable<Track> GetAlbumTracksInDefaultOrder(Artist artist);
    }
}
