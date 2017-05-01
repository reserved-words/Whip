using System.Threading.Tasks;

namespace LastFmApi.Interfaces
{
    public interface ILastFmTrackLoveService
    {
        Task<bool> IsLovedAsync(AuthorizedApiClient client, Track track);
        Task LoveTrackAsync(AuthorizedApiClient client, Track track);
        Task UnloveTrackAsync(AuthorizedApiClient client, Track track);
    }
}
