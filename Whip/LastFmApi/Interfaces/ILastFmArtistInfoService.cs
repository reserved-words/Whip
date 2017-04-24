using System.Threading.Tasks;

namespace LastFmApi.Interfaces
{
    public interface ILastFmArtistInfoService
    {
        Task<ArtistInfo> GetInfo(string artistName);
    }
}
