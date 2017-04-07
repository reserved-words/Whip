﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model
{
    public class Artist
    {
        public Artist()
        {
            City = new City();
            Albums = new List<Album>();
            Tracks = new List<Track>();
        }

        public string Name { get; set; }
        public string Genre { get; set; }
        public string Grouping { get; set; }
        public City City { get; set; }
        public string Website { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }

        public List<Album> Albums { get; set; }
        public List<Track> Tracks { get; set; }
    }
}
