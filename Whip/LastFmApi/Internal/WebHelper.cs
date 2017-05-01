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

            HttpResponseMessage response;

            using (var client = new HttpClient())
            {
                try
                {
                    response = await client.GetAsync(url);
                }
                catch (HttpRequestException ex)
                {
                    throw new LastFmApiException(ErrorCode.ConnectionFailed, ex.Message);
                }
            }
                
            if (!response.IsSuccessStatusCode)
            {
                throw new LastFmApiException(ErrorCode.HttpErrorResponse, response.StatusCode.ToString());
            }

            return await response.Content.ReadAsStringAsync();
        }

        internal static async Task<string> HttpPostAsync(Dictionary<ParameterKey, string> parameters)
        {
            var content = new FormUrlEncodedContent(parameters.ToStringKeys());

            HttpResponseMessage response;

            using (var client = new HttpClient())
            {
                try
                {
                    response = await client.PostAsync(BaseUrl, content);
                }
                catch (HttpRequestException ex)
                {
                    throw new LastFmApiException(ErrorCode.ConnectionFailed, ex.Message);
                }
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new LastFmApiException(ErrorCode.HttpErrorResponse, response.StatusCode.ToString());
            }

            return await response.Content.ReadAsStringAsync();
        }

        private static string BuildUrl(Dictionary<ParameterKey,string> parameters)
        {
            return BaseUrl + "?" + string.Join("&", parameters
                .OrderBy(p => p.GetQueryKey())
                .Select(p => p.GetQueryString()));
        }
    }
}
