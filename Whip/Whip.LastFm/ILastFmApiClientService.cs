using LastFmApi;
using System.Threading.Tasks;

namespace Whip.LastFm
{
    public interface ILastFmApiClientService
    {
        Task SetClients(string apiKey, string apiSecret, string username, string sessionKey);

        ApiClient ApiClient { get; }
        AuthorizedApiClient AuthorizedApiClient { get; }
    }
}
