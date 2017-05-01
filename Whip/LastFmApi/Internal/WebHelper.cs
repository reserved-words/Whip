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
            var url = BuildUrl(parameters);

            using (var client = new HttpClient())
            {
                var task = client.GetAsync(url);

                await task;

                if (task.IsFaulted)
                {
                    throw new LastFmApiException(ErrorCode.ConnectionFailed, task.Exception.InnerException.Message);
                }
                else if (!task.Result.IsSuccessStatusCode)
                {
                    throw new LastFmApiException(ErrorCode.HttpErrorResponse, task.Result.StatusCode.ToString());
                }

                return await task.Result.Content.ReadAsStringAsync();
            }
        }

        internal static async Task<string> HttpPostAsync(Dictionary<ParameterKey, string> parameters)
        {
            var content = new FormUrlEncodedContent(parameters.ToStringKeys());

            using (var client = new HttpClient())
            {
                var task = client.PostAsync(BaseUrl, content);

                await task;

                if (task.IsFaulted)
                {
                    throw new LastFmApiException(ErrorCode.ConnectionFailed, task.Exception.InnerException.Message);
                }
                else if (!task.Result.IsSuccessStatusCode)
                {
                    throw new LastFmApiException(ErrorCode.HttpErrorResponse, task.Result.StatusCode.ToString());
                }

                return await task.Result.Content.ReadAsStringAsync();
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
