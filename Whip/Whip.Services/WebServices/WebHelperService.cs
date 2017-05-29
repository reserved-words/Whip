using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class WebHelperService : IWebHelperService
    {
        public async Task<string> HttpGetAsync(string baseUrl, Dictionary<string, string> parameters)
        {
            var url = BuildUrl(baseUrl, parameters);

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                return await response.Content.ReadAsStringAsync();
            }
        }

        public string UrlEncode(string str)
        {
            return HttpUtility.UrlPathEncode(str);
        }

        private static string BuildUrl(string url, Dictionary<string, string> parameters)
        {
            return url + "?" + string.Join("&", parameters.Select(p => string.Format("{0}={1}", p.Key, p.Value)));
        }
    }
}
