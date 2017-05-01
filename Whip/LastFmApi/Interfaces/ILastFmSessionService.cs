using System.Threading.Tasks;

namespace LastFmApi.Interfaces
{
    public interface ILastFmSessionService
    {
        ApiClient GetApiClient(string apiKey, string secret);
        Task<AuthorizedApiClient> GetAuthorizedApiClientAsync(string apiKey, string secret, string username, string sessionKey);
    }
}
