using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class RssService : IRssService
    {
        private readonly ILoggingService _logger;

        public RssService(ILoggingService logger)
        {
            _logger = logger;
        }

        public async Task PopulatePosts(List<Feed> feeds)
        {
            foreach (var feed in feeds)
            {
                await GetPosts(feed);
            }
        }

        private async Task GetPosts(Feed feed)
        {
            await Task.Run(() =>
            {
                using (var xmlReader = XmlReader.Create(feed.FeedUrl))
                {
                    var sf = SyndicationFeed.Load(xmlReader);

                    feed.Posts = sf.Items
                        .Select(item => new Post
                        {
                            Title = item.Title.Text,
                            Posted = item.PublishDate.DateTime,
                            Url = item.Links.First()?.Uri.ToString(),
                            Feed = feed
                        })
                        .ToList();
                }
            });
        }
    }
}
