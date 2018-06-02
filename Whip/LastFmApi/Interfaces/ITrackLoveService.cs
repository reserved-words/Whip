using System.Threading.Tasks;

namespace LastFmApi.Interfaces
{
    public interface ITrackLoveService
    {
        Task<bool> IsLovedAsync(UserApiClient client, Track track);
        Task LoveTrackAsync(UserApiClient client, Track track);
        Task UnloveTrackAsync(UserApiClient client, Track track);
    }
}
