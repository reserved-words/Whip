using System.Configuration;
using System.IO;
using Whip.Azure;
using Whip.Services.Interfaces;
using static Whip.Common.Resources;

namespace Whip.CloudSync
{
    public class Settings : ILibrarySettings, IAzureStorageConfig
    {
        public string DataDirectory => Path.Combine(MusicDirectory, $"_{ApplicationTitle}");

        public string MusicDirectory
        {
            get { return ConfigurationManager.AppSettings["MusicDirectory"]; }
            set { }
        }

        public string ArchiveDirectory
        {
            get { return null; }
            set { }
        }

        public string AccountName => ConfigurationManager.AppSettings["AccountName"];
        public string ConnectionString => ConfigurationManager.AppSettings["ConnectionString"];
        public string ContainerName => ConfigurationManager.AppSettings["ContainerName"];
    }
}
