using LastFmApi.Internal;
using LastFmApi.Methods.Auth;
using System.Threading;
using System.Threading.Tasks;

namespace LastFmApi
{
    public class UserApiClient : ApiClient
    {
        private const string AuthUrlFormat = "http://www.last.fm/api/auth/?api_key={0}&token={1}";

        private string _authToken;

        internal UserApiClient(string apiKey, string secret, string username, string sessionKey)
            :base(apiKey, secret)
        {
            SessionKey = sessionKey;
            Username = username;
        }

        public string Username { get; private set; }
        public string SessionKey { get; private set; }
        public bool Authorized => !string.IsNullOrEmpty(SessionKey);

        internal async Task GetToken()
        {
            var getTokenMethod = new GetTokenMethod(this);
            _authToken = await getTokenMethod.GetResultAsync();
        }

        internal async Task Authorize()
        {
            if (Authorized)
                return;

            var maxAttempts = 5;
            var attempts = 0;

            Thread.Sleep(10000);

            while (attempts < maxAttempts)
            {
                attempts++;
                Thread.Sleep(2000);

                var getSessionMethod = new GetSessionMethod(this, _authToken);

                try
                {
                    var session = await getSessionMethod.GetResultAsync();

                    if (session.Username != Username)
                    {
                        throw new LastFmApiException(ErrorCode.UserNotLoggedIn, "The requested username must be logged in to authorize access");
                    }

                    SessionKey = session.Key;
                    Username = session.Username;
                    _authToken = null;
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

        public string AuthUrl => string.Format(AuthUrlFormat, ApiKey, _authToken);
    }
}
