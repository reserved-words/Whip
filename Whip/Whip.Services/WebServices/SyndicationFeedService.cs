using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class SyndicationFeedService : ISyndicationFeedService
    {
        public async Task<IEnumerable<SyndicationItem>> GetItemsAsync(string url)
        {
            return await Task.Run(() =>
            {
                using (var xmlReader = XmlReader.Create(url))
                {
                    return SyndicationFeed.Load(xmlReader).Items;
                }
            });
        }
    }
}
