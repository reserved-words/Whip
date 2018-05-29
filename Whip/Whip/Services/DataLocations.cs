using System;
using System.IO;
using Whip.Services.Interfaces;
using static Whip.Common.Resources;

namespace Whip.Services
{
    public class DataLocations : IDataLocations
    {
        private string DataDirectory => Path.Combine(
            Properties.Settings.Default.MusicDirectory, string.Format("_{0}", ApplicationTitle));

        public string GetPath(string filename)
        {
            return Path.Combine(DataDirectory, filename);
        }
    }
}
