using LastFmApi;
using LastFmApi.Interfaces;
using System.Threading.Tasks;

namespace Whip.LastFm
{
    public class LastFmApiClientService : ILastFmApiClientService
    {
        private readonly ISessionService _sessionService;

        private AuthorizedApiClient _authorizedApiClient;
        private ApiClient _apiClient;

        public LastFmApiClientService(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public ApiClient ApiClient => _apiClient;
        public AuthorizedApiClient AuthorizedApiClient => _authorizedApiClient;

        public async Task SetClients(string apiKey, string apiSecret,string username, string sessionKey)
        {
            _apiClient = _sessionService.GetApiClient(apiKey, apiSecret);
            _authorizedApiClient = await _sessionService.GetAuthorizedApiClientAsync(apiKey, apiSecret, username, sessionKey);
        }
    }
}
