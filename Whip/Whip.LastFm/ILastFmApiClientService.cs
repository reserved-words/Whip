using LastFmApi;
using System.Threading.Tasks;

namespace Whip.LastFm
{
    public interface ILastFmApiClientService
    {
        ApiClient ApiClient { get; }
        UserApiClient UserApiClient { get; }

        Task SetClients(string username, string sessionKey);
        Task AuthorizeUserClient();
    }
}
