using System.Threading.Tasks;

namespace LastFmApi.Interfaces
{
    public interface IAlbumInfoService
    {
        Task<string> GetArtworkUrl(ApiClient client, string artistName, string albumTitle);
    }
}
