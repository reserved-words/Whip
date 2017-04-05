using System;
using System.Collections.Generic;

namespace Whip.Common.Model
{
    public class Library
    {
        public Library()
        {
            Artists = new List<Artist>();
        }

        public ICollection<Artist> Artists { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}