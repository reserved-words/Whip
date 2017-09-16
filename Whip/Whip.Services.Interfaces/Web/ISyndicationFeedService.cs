using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;

namespace Whip.Services.Interfaces
{
    public interface ISyndicationFeedService
    {
        Task<IEnumerable<SyndicationItem>> GetItemsAsync(string url);
    }
}
