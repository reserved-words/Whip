using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface IWebArtistInfoService
    {
        Task<ArtistWebInfo> PopulateArtistImages(Artist artist);
    }
}
