﻿using LastFmApi.Interfaces;

namespace LastFmApi
{
    public class LastFmSessionService : ILastFmSessionService
    {
        public ApiClient GetApiClient(string apiKey, string secret)
        {
            return new ApiClient(apiKey, secret);
        }

        public AuthorizedApiClient GetAuthorizedApiClient(string apiKey, string secret, string username, string sessionKey)
        {
            var client = new AuthorizedApiClient(apiKey, secret, username, sessionKey);
            client.GenerateSessionKey();
            return client;
        }
    }
}