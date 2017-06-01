using System.Threading.Tasks;

namespace Whip.Services.Interfaces
{
    public interface IAlbumInfoService
    {
        Task<string> GetArtworkUrl(string artistName, string albumTitle);
    }
}
