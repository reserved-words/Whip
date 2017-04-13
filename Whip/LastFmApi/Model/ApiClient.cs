using LastFmApi.Methods;
using System;
using System.Threading;

namespace LastFmApi
{
    public class ApiClient
    {
        internal ApiClient(string apiKey, string secret)
        {
            ApiKey = apiKey;
            ApiSecret = secret;
        }

        internal string ApiKey { get; private set; }
        internal string ApiSecret { get; private set; }
    }
}
