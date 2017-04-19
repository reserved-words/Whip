using System.Threading.Tasks;

namespace Whip.Services.Interfaces
{
    public interface IWebAlbumInfoService
    {
        Task<string> GetArtworkUrl(string artistName, string albumTitle);
    }
}
