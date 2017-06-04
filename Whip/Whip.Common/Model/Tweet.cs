using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model
{
    public class Tweet
    {
        public DateTime Posted { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string UserImage { get; set; }
        public string Image { get; set; }
        public bool IsRetweet { get; set; }
        public string Url { get; set; }
        public string UserUrl { get; set; }
    }
}
