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
        public string Artwork { get; set; }

        public Artist Artist { get; set; }

        public List<Disc> Discs { get; set; }


        public bool MultiDisc => Discs.Count > 1;
    }
}
