using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using Whip.Common.Model.Rss;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class RssService : IRssService
    {
        public void PopulatePosts(List<Feed> feeds)
        {
            foreach (var feed in feeds)
            {
                GetPosts(feed);
            }
        }

        private void GetPosts(Feed feed)
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
        }
    }
}
