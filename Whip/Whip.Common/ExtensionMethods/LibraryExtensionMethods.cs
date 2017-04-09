using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.Singletons;

namespace Whip.Common.ExtensionMethods
{
    public static class LibraryExtensionMethods
    {
        public static List<string> GetGroupings(this Library library)
        {
            return library.Artists
                .Select(a => a.Grouping)
                .Distinct()
                .OrderBy(g => g)
                .ToList();
        }
    }
}
