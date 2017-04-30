using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LastFmApi.Internal
{
    internal class WebHelper
    {
        private const string BaseUrl = "https://ws.audioscrobbler.com/2.0/";

        internal static async Task<string> HttpGetAsync(string baseUrl, Dictionary<ParameterKey, string> parameters)
        {
            using (var client = new HttpClient())
            {
                var url = BuildUrl(parameters);

                var response = client.TryGetAsync(url).Result;

                var responseString = await response.Content.ReadAsStringAsync();

                return responseString;
            }
        }

        internal static async Task<string> HttpGetAsync(Dictionary<ParameterKey, string> parameters)
        {
            using (var client = new HttpClient())
            {
                var url = BuildUrl(parameters);

                var response = client.TryGetAsync(url).Result;

                var responseString = await response.Content.ReadAsStringAsync();

                return responseString;
            }
        }

        internal static async Task<string> HttpPostAsync(Dictionary<ParameterKey, string> parameters)
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(parameters.ToStringKeys());

                var response = await client.TryPostAsync(BaseUrl, content);

                var responseString = await response.Content.ReadAsStringAsync();

                return responseString;
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
