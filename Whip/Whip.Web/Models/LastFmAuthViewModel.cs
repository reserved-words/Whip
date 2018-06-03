using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Whip.Web.Models
{
    public class LastFmAuthViewModel
    {
        public LastFmAuthViewModel(string url, bool failed = false)
        {
            Url = url;
            Failed = failed;
        }

        public string Url { get; }
        public bool Failed { get; }
    }
}