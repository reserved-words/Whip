using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Whip.Common.ExtensionMethods;
using Whip.Common.Model.Rss;
using Whip.Services.Interfaces;
using static Whip.Services.XmlPropertyNames;

namespace Whip.Services
{
    public class XmlRssFeedsRepository : IRssFeedsRepository
    {
        private const string Filename = "rss.xml";

        private readonly IUserSettings _userSettings;

        public XmlRssFeedsRepository(IUserSettings userSettings)
        {
            _userSettings = userSettings;
        }

        private string XmlFilePath => Path.Combine(_userSettings.DataDirectory, Filename);

        public List<Feed> GetFeeds()
        {
            var feeds = new List<Feed>();

            if (!File.Exists(XmlFilePath))
            {
                return feeds;
            }

            var xml = XDocument.Load(XmlFilePath);
            
            var feedsXml = xml.Root.Element(RssFeeds);

            foreach (var feed in feedsXml.Elements(RssFeed))
            {
                feeds.Add(new Feed(
                    feed.Attribute(RssTitle).Value,
                    feed.Attribute(RssUrl).Value,
                    feed.Attribute(RssFeedUrl).Value
                ));
            }

            return feeds;
        }

        public void SaveFeeds(List<Feed> feeds)
        {
            var xml = new XDocument();

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
                feedsXml.Add(feedXml);
            }

            Directory.CreateDirectory(_userSettings.DataDirectory);

            xml.Save(XmlFilePath);
        }

    }
}