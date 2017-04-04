using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model
{
    public class Album
    {
        public Album()
        {
            Discs = new List<Disc>();
        }

        public string Title { get; set; }
        public string Year { get; set; }

        public Artist Artist { get; set; }

        public ICollection<Disc> Discs { get; set; }
    }
}
