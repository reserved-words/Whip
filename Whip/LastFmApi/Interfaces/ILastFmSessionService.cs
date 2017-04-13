using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastFmApi.Interfaces
{
    public interface ILastFmSessionService
    {
        ApiClient GetApiClient(string apiKey, string secret);
        AuthorizedApiClient GetAuthorizedApiClient(string apiKey, string secret, string username, string sessionKey);
    }
}
