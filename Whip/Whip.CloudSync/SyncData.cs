using System;
using System.IO;
using System.Xml.Linq;
using Whip.Services.Interfaces;

namespace Whip.CloudSync
{
    public class SyncData
    {
        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        private readonly ILibrarySettings _settings;

        public SyncData(ILibrarySettings settings)
        {
            _settings = settings;
        }

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

        private XDocument GetSyncData()
        {
            XDocument doc;
            var syncDataPath = GetSyncDataPath();
            if (!File.Exists(syncDataPath))
            {
                doc = CreateSyncDataDocument();
                doc.Save(syncDataPath);
            }
            else
            {
                doc = XDocument.Load(syncDataPath);
            }
            return doc;
        }

        private static XDocument CreateSyncDataDocument()
        {
            var doc = new XDocument();
            var root = new XElement("syncdata");
            var lastSynced = new XElement("last_synced", DateTime.MinValue.ToString(DateTimeFormat));
            root.Add(lastSynced);
            doc.Add(root);
            return doc;
        }

        private string GetSyncDataPath()
        {
            return Path.Combine(_settings.DataDirectory, "syncdata.xml");
        }
    }
}
