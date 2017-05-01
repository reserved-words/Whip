using LastFmApi.Internal;
using LastFmApi.Methods.Auth;
using System;
using System.Threading;
using System.Threading.Tasks;

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

        internal async Task GenerateSessionKeyAsync()
        {
            if (!string.IsNullOrEmpty(SessionKey))
                return;

            var getTokenMethod = new GetTokenMethod(this);
            var token = await getTokenMethod.GetResultAsync();

            RequestAuthorisation(token);

            var maxAttempts = 5;

            var attempts = 0;

            while (attempts < maxAttempts)
            {
                attempts++;

                Thread.Sleep(2000);

                var getSessionMethod = new GetSessionMethod(this, token);

                try
                {
                    var session = await getSessionMethod.GetResultAsync();

                    if (session.Username != Username)
                    {
                        throw new LastFmApiException(ErrorCode.UserNotLoggedIn, "The requested username must be logged in to authorize access");
                    }

                    SessionKey = session.Key;
                    Username = session.Username;
                    break;
                }
                catch (LastFmApiException ex)
                {
                    if (ex.ErrorCode == ErrorCode.HttpErrorResponse)
                    {
                        continue;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        internal void RequestAuthorisation(string token)
        {
            string url = string.Format(AuthUrl, ApiKey, token);
            System.Diagnostics.Process.Start(url);
        }
    }
}
