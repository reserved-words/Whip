using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model
{
    public class City
    {
        private readonly Lazy<string> _description;
        
        public City()
            : this(string.Empty, string.Empty, string.Empty)
        {

        }

        public City(string name, string state, string country)
        {
            Name = name;
            State = state;
            Country = country;

            _description = new Lazy<string>(FormatDescription);
        }

        public string Name { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }

        public string Description => _description.Value;

        private string FormatDescription()
        {
            var array = new string[] { Name, State, Country };

            return string.Join(", ", array.Where(s => !string.IsNullOrEmpty(s)));
        }
    }
}
