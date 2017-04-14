using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class LibrarySortingService : ILibrarySortingService
    {
        public IOrderedEnumerable<Artist> GetInDefaultOrder(IEnumerable<Artist> artists)
        {
            return artists.OrderBy(a => Alphabetise(a.Name));
        }

        private string Alphabetise(string name)
        {
            return name.Replace("The ", "");
        }
    }
}
