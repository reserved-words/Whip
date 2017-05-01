using LastFmApi;
using LastFmApi.Interfaces;
using System;
using System.Threading.Tasks;
using Whip.Services.Interfaces;

namespace Whip.LastFm
{
    public class LastFmApiClientService : ILastFmApiClientService
    {
        private readonly ILastFmSessionService _sessionService;
        private readonly IUserSettings _userSettings;

        private readonly Lazy<Task<AuthorizedApiClient>> _authorizedApiClient;
        private readonly Lazy<ApiClient> _apiClient;

        public LastFmApiClientService(ILastFmSessionService sessionService, IUserSettings userSettings)
        {
            _sessionService = sessionService;
            _userSettings = userSettings;

            _authorizedApiClient = new Lazy<Task<AuthorizedApiClient>>(GetAuthorizedApiClientAsync);
            _apiClient = new Lazy<ApiClient>(GetApiClient);
        }

        public ApiClient ApiClient => _apiClient.Value;
        public Task<AuthorizedApiClient> AuthorizedApiClient => _authorizedApiClient.Value;

        private async Task<AuthorizedApiClient> GetAuthorizedApiClientAsync()
        {
            var client = await _sessionService.GetAuthorizedApiClientAsync(
                _userSettings.LastFmApiKey,
                _userSettings.LastFmApiSecret,
                _userSettings.LastFmUsername,
                _userSettings.LastFmApiSessionKey);

            _userSettings.LastFmUsername = client.Username;
            _userSettings.LastFmApiSessionKey = client.SessionKey;
            _userSettings.Save();

            return client;
        }

        private ApiClient GetApiClient()
        {
            return _sessionService.GetApiClient(
                _userSettings.LastFmApiKey,
                _userSettings.LastFmApiSecret);
        }
    }
}
