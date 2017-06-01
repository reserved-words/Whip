using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface IArtistInfoService
    {
        Task<bool> PopulateArtistInfo(Artist artist, int numberOfSimilarArtists);
    }
}
