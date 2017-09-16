using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class RssService : IRssService
    {
        private readonly ISyndicationFeedService _syndicationFeedService;

        public RssService(ISyndicationFeedService syndicationFeedService)
        {
            _syndicationFeedService = syndicationFeedService;
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
            var syndicationItems = await _syndicationFeedService
                .GetItemsAsync(feed.FeedUrl);

            feed.Posts = syndicationItems
                .Select(item => new Post
                {
                    Title = item.Title.Text,
                    Posted = item.PublishDate.DateTime,
                    Url = item.Links.First()?.Uri.ToString(),
                    Feed = feed
                })
                .ToList();
        }
    }
}
