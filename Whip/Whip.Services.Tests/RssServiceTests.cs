using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services.Tests
{
    [TestClass]
    public class RssServiceTests
    {
        private readonly DateTimeOffset[] _dates = 
        {
            new DateTimeOffset(2017,1,1,15,30,15,0,TimeSpan.Zero),
            new DateTimeOffset(2016,2,11,19,36,57,0,TimeSpan.Zero),
            new DateTimeOffset(2016,5,30,02,45,59,0,TimeSpan.Zero),
        };

        private readonly string[] _urls = 
        {
            "https://www.test.com/1",
            "http://www.test.com/2315",
            "https://www.test.com/abc"
        };

        [TestMethod]
        public void PopulatePosts_PopulatesPosts()
        {
            // Arrange
            var feed1 = new Feed("Feed 1", "", "theh", "", "");
            var feed2 = new Feed("Feed 2", "", "456789", "", "");
            var feed3 = new Feed("Feed 3", "", "test", "", "");
            var feeds = new List<Feed> { feed1, feed2, feed3 };

            var posts1 = GetItems(0);
            var posts2 = GetItems(1, 2);
            var posts3 = GetItems();

            var mockSyndicationFeedService = new Mock<ISyndicationFeedService>();
            mockSyndicationFeedService.Setup(s => s.GetItemsAsync(feed1.FeedUrl)).ReturnsAsync(posts1);
            mockSyndicationFeedService.Setup(s => s.GetItemsAsync(feed2.FeedUrl)).ReturnsAsync(posts2);
            mockSyndicationFeedService.Setup(s => s.GetItemsAsync(feed3.FeedUrl)).ReturnsAsync(posts3);

            var sut = new RssService(mockSyndicationFeedService.Object);

            // Act
            sut.PopulatePosts(feeds).GetAwaiter();

            // Assert
            feed1.Posts.Count.Should().Be(posts1.Count);
            var post = feed1.Posts.Single();
            post.Title.Should().Be(posts1[0].Title.Text);
            post.Posted.Should().Be(_dates[0].DateTime);
            post.Url.Should().Be(_urls[0]);
            post.Feed.Should().Be(feed1);

            feed2.Posts.Count.Should().Be(posts2.Count);

            var post1 = feed2.Posts.First();
            post1.Title.Should().Be(posts2[0].Title.Text);
            post1.Posted.Should().Be(_dates[1].DateTime);
            post1.Url.Should().Be(_urls[1]);
            post1.Feed.Should().Be(feed2);

            var post2 = feed2.Posts.Last();
            post2.Title.Should().Be(posts2[1].Title.Text);
            post2.Posted.Should().Be(_dates[2].DateTime);
            post2.Url.Should().Be(_urls[2]);
            post2.Feed.Should().Be(feed2);

            feed3.Posts.Count.Should().Be(0);
        }

        private List<SyndicationItem> GetItems(params int[] ids)
        {
            return ids.Select(GetItem).ToList();
        }

        private SyndicationItem GetItem(int index)
        {
            var item = new SyndicationItem($"Title {index}", $"Content {index}", new Uri(_urls[index]))
            {
                PublishDate = _dates[index]
            };

            item.Links.Add(new SyndicationLink(new Uri(_urls[index])));
            item.Links.Add(new SyndicationLink(new Uri("http://dummylink.com/")));

            return item;
        }
    }
}
