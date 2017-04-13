using LastFmApi.Internal;
using LastFmApi.Methods.Auth;
using System;
using System.Threading;

namespace LastFmApi
{
    public class AuthorizedApiClient : ApiClient
    {
        private const string AuthUrl = "http://www.last.fm/api/auth/?api_key={0}&token={1}";

        internal AuthorizedApiClient(string apiKey, string secret, string username, string sessionKey)
            :base(apiKey, secret)
        {
            SessionKey = sessionKey;
            Username = username;
        }

        public string Username { get; private set; }
        public string SessionKey { get; private set; }

        internal void GenerateSessionKey()
        {
            if (!string.IsNullOrEmpty(SessionKey))
                return;

            var getTokenMethod = new GetTokenMethod(this);
            var token = getTokenMethod.GetResult();

            RequestAuthorisation(token);

            var maxAttempts = 5;

            var attempts = 0;

            while (attempts < maxAttempts)
            {
                attempts++;

                Thread.Sleep(2000);

                var getSessionMethod = new GetSessionMethod(this, token);
                var session = getSessionMethod.GetResult();

                if (session == null)
                    continue;

                if (session.Username != Username)
                {
                    throw new Exception("Incorrect user logged in");
                }

                SessionKey = session.Key;
                Username = session.Username;
                break;
            }
        }

        internal void RequestAuthorisation(string token)
        {
            string url = string.Format(AuthUrl, ApiKey, token);
            System.Diagnostics.Process.Start(url);
        }
    }
}
