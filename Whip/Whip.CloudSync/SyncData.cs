using System;
using System.Configuration;
using System.IO;
using System.Xml.Linq;
using Whip.Services.Interfaces;

namespace Whip.CloudSync
{
    public class SyncData : IDataLocations, ICloudStorageConfig
    {
        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        public DateTime GetTimeLastSynced()
        {
            var doc = GetSyncData();
            var root = doc.Element("syncdata");
            var lastSynced = root.Element("last_synced");
            return DateTime.Parse(lastSynced.Value);
        }

        public void SetTimeLastSynced(DateTime time)
        {
            var doc = GetSyncData();
            var root = doc.Element("syncdata");
            var lastSynced = root.Element("last_synced");
            lastSynced.SetValue(time.ToString(DateTimeFormat));
            doc.Save(GetSyncDataPath());
        }

        private string DataDirectory => ConfigurationManager.AppSettings["DataDirectory"];
        public string AccountName => ConfigurationManager.AppSettings["CloudAccountName"];
        public string ConnectionString => ConfigurationManager.AppSettings["CloudConnectionString"];
        public string ContainerName => ConfigurationManager.AppSettings["CoudContainerName"];

        private XDocument GetSyncData()
        {
            var syncDataPath = GetSyncDataPath();
            return XDocument.Load(syncDataPath);
        }

        private string GetSyncDataPath()
        {
            return Path.Combine(DataDirectory, "syncdata.xml");
        }

        public string GetPath(string filename)
        {
            return Path.Combine(DataDirectory, filename);
        }
    }
}
