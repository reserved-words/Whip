using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class WebHelperService : IWebHelperService
    {
        public async Task<string> HttpGetAsync(string baseUrl, Dictionary<string, string> parameters = null)
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

        private static string BuildUrl(string url, Dictionary<string, string> parameters = null)
        {
            if (parameters == null)
                return url;

            return url + "?" + string.Join("&", parameters.Select(p => string.Format("{0}={1}", p.Key, p.Value)));
        }

        public async Task HttpPostAsync(string url, object data)
        {
            using (var client = new HttpClient())
            {
                var content = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await client.PostAsync(url, byteContent);
            }
        }
    }
}
