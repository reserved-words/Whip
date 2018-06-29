using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Whip.Web.Models
{
    public class PlayItemViewModel
    {
        public PlayItemViewModel(string title, string playUrl, string infoUrl)
        {
            Title = title;
            PlayUrl = playUrl;
            InfoUrl = infoUrl;
        }

        public string Title { get; }
        public string PlayUrl { get; }
        public string InfoUrl { get; }
    }
}