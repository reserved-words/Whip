using LastFmApi;

namespace Whip.LastFm
{
    public interface ILastFmApiClientService
    {
        ApiClient ApiClient { get; }
        AuthorizedApiClient AuthorizedApiClient { get; }
    }
}
