using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface ILibrarySortingService
    {
        IOrderedEnumerable<Artist> GetInDefaultOrder(IEnumerable<Artist> artists);

        void SortDiscs(Album album);

        void SortTracks(Disc disc);

        string SortValue(Artist artist);

        string SortValue(Album album);
    }
}
