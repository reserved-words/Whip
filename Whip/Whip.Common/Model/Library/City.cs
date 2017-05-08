using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model
{
    public struct City
    {
        public City(string name, string state, string country)
        {
            Name = name;
            State = state;
            Country = country;
        }

        public string Name { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }

        public string Description => FormatDescription();

        private string FormatDescription()
        {
            var array = new string[] { Name, State, Country };

            return string.Join(", ", array.Where(s => !string.IsNullOrEmpty(s)));
        }
    }
}
