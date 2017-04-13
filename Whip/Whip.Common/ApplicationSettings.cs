using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common
{
    public static class ApplicationSettings
    {
        public static string[] FileExtensions => new[] { ".mp3", ".m4a" };
        public static int TrackChangeDelay => 800;
    }
}
