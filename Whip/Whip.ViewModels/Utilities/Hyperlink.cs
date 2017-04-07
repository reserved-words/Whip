using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.ViewModels.Utilities
{
    public static class Hyperlink
    {
        public static void Go(string url)
        {
            System.Diagnostics.Process.Start(url);
        }
    }
}
