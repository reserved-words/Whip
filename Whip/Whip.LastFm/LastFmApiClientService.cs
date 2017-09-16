using LastFmApi;
using LastFmApi.Interfaces;
using System.Threading.Tasks;
using Whip.Services.Interfaces.Singletons;

namespace Whip.LastFm
{
    public class LastFmApiClientService : ILastFmApiClientService
    {
        private readonly ISessionService _sessionService;
        private readonly IConfigSettings _configSettings;

        private AuthorizedApiClient _authorizedApiClient;
        private ApiClient _apiClient;

        public LastFmApiClientService(ISessionService sessionService, IConfigSettings configSettings)
        {
            _configSettings = configSettings;
            _sessionService = sessionService;
        }

        public ApiClient ApiClient => _apiClient;
        public AuthorizedApiClient AuthorizedApiClient => _authorizedApiClient;

        public async Task SetClients(string username, string sessionKey)
        {
            _apiClient = _sessionService.GetApiClient(_configSettings.LastFmApiKey, _configSettings.LastFmApiSecret);
            _authorizedApiClient = await _sessionService.GetAuthorizedApiClientAsync(_configSettings.LastFmApiKey, _configSettings.LastFmApiSecret, username, sessionKey);
        }
    }
}
