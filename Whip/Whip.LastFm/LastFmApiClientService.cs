using LastFmApi;
using LastFmApi.Interfaces;
using System;
using Whip.Services.Interfaces;

namespace Whip.LastFm
{
    public class LastFmApiClientService : ILastFmApiClientService
    {
        private readonly ILastFmSessionService _sessionService;
        private readonly IUserSettings _userSettings;

        private readonly Lazy<AuthorizedApiClient> _authorizedApiClient;
        private readonly Lazy<ApiClient> _apiClient;

        public LastFmApiClientService(ILastFmSessionService sessionService, IUserSettings userSettings)
        {
            _sessionService = sessionService;
            _userSettings = userSettings;

            _authorizedApiClient = new Lazy<AuthorizedApiClient>(GetAuthorizedApiClient);
            _apiClient = new Lazy<ApiClient>(GetAuthorizedApiClient);
        }

        public ApiClient ApiClient => _apiClient.Value;
        public AuthorizedApiClient AuthorizedApiClient => _authorizedApiClient.Value;

        private AuthorizedApiClient GetAuthorizedApiClient()
        {
            var client = _sessionService.GetAuthorizedApiClient(
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
