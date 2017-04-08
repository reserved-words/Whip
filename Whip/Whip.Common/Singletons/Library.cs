using System;
using System.Collections.Generic;
using Whip.Common.Model;

namespace Whip.Common.Singletons
{
    public class Library
    {
        public event Action Updated;

        public Library()
        {
            Artists = new List<Artist>();
        }

        public List<Artist> Artists { get; set; }
        public DateTime LastUpdated { get; set; }

        public void Update(Library library)
        {
            LastUpdated = library.LastUpdated;
            Artists = library.Artists;
            Updated?.Invoke();
        }
    }
}