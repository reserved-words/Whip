using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model
{
    public class ArtistEvent
    {
        public DateTime Date { get; set; }
        public string Venue { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ArtistList { get; set; }
    }
}
