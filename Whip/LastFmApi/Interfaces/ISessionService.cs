using System.Threading.Tasks;

namespace LastFmApi.Interfaces
{
    public interface ISessionService
    {
        ApiClient GetApiClient(string apiKey, string secret);
        Task<UserApiClient> GetAuthorizedApiClientAsync(string apiKey, string secret, string username, string sessionKey);
        Task Authorize(UserApiClient client, int maxAttempts);
    }
}
