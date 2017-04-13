using LastFmApi;
using LastFmApi.Interfaces;
using System;
using Whip.Services.Interfaces;

namespace Whip.LastFm
{
    public class LastFmApiClientService : ILastFmApiClientService
    {
        private readonly ILastFmSessionService _sessionService;
        private readonly IUserSettingsService _userSettingsService;

        private readonly Lazy<AuthorizedApiClient> _authorizedApiClient;
        private readonly Lazy<ApiClient> _apiClient;

        public LastFmApiClientService(ILastFmSessionService sessionService, IUserSettingsService userSettingsService)
        {
            _sessionService = sessionService;
            _userSettingsService = userSettingsService;

            _authorizedApiClient = new Lazy<AuthorizedApiClient>(GetAuthorizedApiClient);
            _apiClient = new Lazy<ApiClient>(GetAuthorizedApiClient);
        }

        public ApiClient ApiClient => _apiClient.Value;
        public AuthorizedApiClient AuthorizedApiClient => _authorizedApiClient.Value;

        private AuthorizedApiClient GetAuthorizedApiClient()
        {
            var client = _sessionService.GetAuthorizedApiClient(
                _userSettingsService.LastFmApiKey,
                _userSettingsService.LastFmApiSecret,
                _userSettingsService.LastFmUsername,
                _userSettingsService.LastFmApiSessionKey);

            _userSettingsService.LastFmUsername = client.Username;
            _userSettingsService.LastFmApiSessionKey = client.SessionKey;
            _userSettingsService.Save();

            return client;
        }

        private ApiClient GetApiClient()
        {
            return _sessionService.GetApiClient(
                _userSettingsService.LastFmApiKey,
                _userSettingsService.LastFmApiSecret);
        }
    }
}
