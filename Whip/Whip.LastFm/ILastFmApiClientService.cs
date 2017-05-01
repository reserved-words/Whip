using LastFmApi;
using System.Threading.Tasks;

namespace Whip.LastFm
{
    public interface ILastFmApiClientService
    {
        ApiClient ApiClient { get; }
        Task<AuthorizedApiClient> AuthorizedApiClient { get; }
    }
}
