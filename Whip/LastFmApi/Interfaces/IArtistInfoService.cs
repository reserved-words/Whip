using System.Threading.Tasks;

namespace LastFmApi.Interfaces
{
    public interface IArtistInfoService
    {
        Task<ArtistInfo> GetInfo(ApiClient client, string artistName);
    }
}
