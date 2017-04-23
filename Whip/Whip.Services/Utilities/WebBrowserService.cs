using System.Linq;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class WebBrowserService : IWebBrowserService
    {
        private const string SearchBaseUrl = "https://www.google.co.uk/?gws_rd=ssl#q=";

        public void Open(string url)
        {
            System.Diagnostics.Process.Start(url);
        }

        public void OpenSearch(params string[] searchTerms)
        {
            var queryString = string.Join("+", searchTerms.Select(str => 
                str.Contains(" ") 
                    ? string.Format("%22{0}%22", str.Replace(" ", "+")) 
                    : str));

            Open(SearchBaseUrl + queryString);
        }
    }
}
