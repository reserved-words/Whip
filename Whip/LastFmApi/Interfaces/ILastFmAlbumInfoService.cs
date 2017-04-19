using System.Threading.Tasks;

namespace LastFmApi.Interfaces
{
    public interface ILastFmAlbumInfoService
    {
        Task<string> GetArtworkUrl(string artistName, string albumTitle);
    }
}
