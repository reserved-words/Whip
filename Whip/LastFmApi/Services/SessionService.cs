using LastFmApi.Interfaces;
using System.Threading.Tasks;

namespace LastFmApi
{
    public class SessionService : ISessionService
    {
        public ApiClient GetApiClient(string apiKey, string secret)
        {
            return new ApiClient(apiKey, secret);
        }

        public async Task<UserApiClient> GetAuthorizedApiClientAsync(string apiKey, string secret, string username, string sessionKey)
        {
            var client = new UserApiClient(apiKey, secret, username, sessionKey);

            if (!client.Authorized)
            {
                await client.GetToken();
            }

            return client;
        }

        public async Task Authorize(UserApiClient client)
        {
            await client.Authorize();
        }
    }
}
