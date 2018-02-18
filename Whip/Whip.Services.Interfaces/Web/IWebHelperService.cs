using System.Collections.Generic;
using System.Threading.Tasks;

namespace Whip.Services.Interfaces
{
    public interface IWebHelperService
    {
        Task<string> HttpGetAsync(string url, Dictionary<string, string> parameters = null);

        Task HttpPostAsync(string url, object data);

        string UrlEncode(string str);
    }
}
