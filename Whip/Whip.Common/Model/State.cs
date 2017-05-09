using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model
{
    public class State
    {
        public State(City city)
        {
            Name = city.State;
            Country = city.Country;
        }

        public string Description => string.Format("{0}, {1}", Name, Country);

        public string Name { get; private set; }
        public string Country { get; private set; }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            var state = obj as State;

            if (state == null)
                return false;

            return state.Name == Name && state.Country == Country;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Country.GetHashCode();
        }
    }
}
