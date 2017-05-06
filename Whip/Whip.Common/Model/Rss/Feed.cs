using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model.Rss
{
    public class Feed
    {
        public Feed(string title, string url, string feedUrl)
        {
            Title = title;
            Url = url;
            FeedUrl = feedUrl;

            Posts = new List<Post>();
        }

        public string Title { get; set; }

        public string Url { get; set; }

        public string FeedUrl { get; set; }

        public ICollection<Post> Posts { get; set; }

        public void AddPost(Post post)
        {
            Posts.Add(post);
            post.Feed = this;
            post.Url = string.Format("{0}{1}", Url, post.Title);  // temp
        }
    }
}
