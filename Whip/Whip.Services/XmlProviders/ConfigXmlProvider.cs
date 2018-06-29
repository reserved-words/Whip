using System.IO;
using System.Xml.Linq;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class ConfigXmlProvider : IXmlProvider
    {
        private readonly IDataLocations _settings;

        public ConfigXmlProvider(IDataLocations settings)
        {
            _settings = settings;
        }

        private string XmlFilePath => _settings.GetPath("config.xml");

        public XDocument Get()
        {
            return File.Exists(XmlFilePath)
                ? XDocument.Load(XmlFilePath)
                : null;
        }

        public void Save(XDocument doc)
        {
            doc.Save(XmlFilePath);
        }
    }
}
