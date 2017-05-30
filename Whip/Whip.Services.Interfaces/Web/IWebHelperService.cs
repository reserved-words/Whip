using System.Collections.Generic;
using System.Threading.Tasks;

namespace Whip.Services.Interfaces
{
    public interface IWebHelperService
    {
        Task<string> HttpGetAsync(string url, Dictionary<string, string> parameters = null);

        string UrlEncode(string str);
    }
}
