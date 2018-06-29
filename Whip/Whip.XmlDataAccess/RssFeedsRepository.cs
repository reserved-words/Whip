using System.Collections.Generic;
using System.Xml.Linq;
using Whip.Common.ExtensionMethods;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using static Whip.XmlDataAccess.PropertyNames;

namespace Whip.XmlDataAccess
{
    public class RssFeedsRepository : IRssFeedsRepository
    {
        private readonly IXmlProvider _xmlProvider;

        public RssFeedsRepository(IXmlProvider xmlProvider)
        {
            _xmlProvider = xmlProvider;
        }

        public List<Feed> GetFeeds()
        {
            var feeds = new List<Feed>();

            var xml = _xmlProvider.Get();

            if (xml == null)
            {
                return feeds;
            }

            var feedsXml = xml.Root.Element(RssFeeds);

            foreach (var feed in feedsXml.Elements(RssFeed))
            {
                feeds.Add(new Feed(
                    feed.Attribute(RssTitle).Value,
                    feed.Attribute(RssUrl).Value,
                    feed.Attribute(RssFeedUrl).Value,
                    feed.Attribute(RssIconUrl).Value,
                    feed.Attribute(RssColor).Value
                ));
            }

            return feeds;
        }

        public void SaveFeeds(List<Feed> feeds)
        {
            var xml = _xmlProvider.Get();

            var rootXml = new XElement(RssRoot);
            xml.Add(rootXml);
            
            var feedsXml = new XElement(RssFeeds);
            rootXml.Add(feedsXml);

            foreach (var feed in feeds)
            {
                var feedXml = new XElement(RssFeed);
                feedXml.AddAttribute(RssTitle, feed.Title);
                feedXml.AddAttribute(RssUrl, feed.Url);
                feedXml.AddAttribute(RssFeedUrl, feed.FeedUrl);
                feedXml.AddAttribute(RssIconUrl, feed.IconUrl);
                feedXml.AddAttribute(RssColor, feed.Color);
                feedsXml.Add(feedXml);
            }

            _xmlProvider.Save(xml);
        }

    }
}