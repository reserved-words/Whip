using System.Threading.Tasks;

namespace LastFmApi.Interfaces
{
    public interface ILastFmTrackLoveService
    {
        Task<bool> IsLovedAsync(Track track);
        Task LoveTrackAsync(Track track);
        Task UnloveTrackAsync(Track track);
    }
}
