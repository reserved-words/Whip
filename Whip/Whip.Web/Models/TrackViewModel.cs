using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Whip.Common.Model;

namespace Whip.Web.Models
{
    public class TrackViewModel
    {
        public TrackViewModel(Track track, string url, string artworkUrl)
        {
            Track = track;
            Url = url;
            ArtworkUrl = artworkUrl;
        }

        public Track Track { get; set; }
        public string Url { get; set; }
        public string ArtworkUrl { get; set; }
    }
}