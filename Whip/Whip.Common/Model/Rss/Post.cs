using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model.Rss
{
    public class Post
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public DateTime Posted { get; set; }

        public Feed Feed { get; set; }
    }
}
