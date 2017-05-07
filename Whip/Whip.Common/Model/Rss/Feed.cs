using System.Collections.Generic;

namespace Whip.Common.Model.Rss
{
    public class Feed
    {
        public Feed() { }

        public Feed(string title, string url, string feedUrl, string iconUrl, string color)
        {
            Title = title;
            Url = url;
            FeedUrl = feedUrl;
            IconUrl = iconUrl;
            Color = color;

            Posts = new List<Post>();
        }

        public string Title { get; set; }

        public string Url { get; set; }

        public string FeedUrl { get; set; }

        public string IconUrl { get; set; }

        public string Color { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
