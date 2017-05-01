using LastFmApi.Interfaces;
using System.Threading.Tasks;

namespace LastFmApi
{
    public class LastFmSessionService : ILastFmSessionService
    {
        public ApiClient GetApiClient(string apiKey, string secret)
        {
            return new ApiClient(apiKey, secret);
        }

        public async Task<AuthorizedApiClient> GetAuthorizedApiClientAsync(string apiKey, string secret, string username, string sessionKey)
        {
            var client = new AuthorizedApiClient(apiKey, secret, username, sessionKey);
            await client.GenerateSessionKeyAsync();
            return client;
        }
    }
}
