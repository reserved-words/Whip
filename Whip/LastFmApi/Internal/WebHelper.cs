using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LastFmApi.Internal
{
    internal class WebHelper
    {
        private const string BaseUrl = "https://ws.audioscrobbler.com/2.0/";

        internal static async Task<string> HttpGetAsync(Dictionary<ParameterKey, string> parameters)
        {
            using (var client = new HttpClient())
            {
                var url = BuildUrl(parameters);

                HttpResponseMessage response = null;

                await client.GetAsync(url).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        throw new LastFmApiException(ErrorCode.ConnectionFailed, t.Exception.InnerException.Message);
                    }
                    else
                    {
                        if (!t.Result.IsSuccessStatusCode)
                        {
                            throw new LastFmApiException(ErrorCode.HttpErrorResponse, response.StatusCode.ToString());
                        }

                        response = t.Result;
                    }
                });

                return await response.Content.ReadAsStringAsync();
            }
        }

        internal static async Task<string> HttpPostAsync(Dictionary<ParameterKey, string> parameters)
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(parameters.ToStringKeys());

                HttpResponseMessage response = null;

                await client.PostAsync(BaseUrl, content).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        throw new LastFmApiException(ErrorCode.ConnectionFailed, t.Exception.InnerException.Message);
                    }
                    else
                    {
                        if (!t.Result.IsSuccessStatusCode)
                        {
                            throw new LastFmApiException(ErrorCode.HttpErrorResponse, response.StatusCode.ToString());
                        }

                        response = t.Result;
                    }
                });

                return await response.Content.ReadAsStringAsync();
            }
        }

        private static string BuildUrl(Dictionary<ParameterKey,string> parameters)
        {
            return BaseUrl + "?" + string.Join("&", parameters
                .OrderBy(p => p.GetQueryKey())
                .Select(p => p.GetQueryString()));
        }
    }
}
