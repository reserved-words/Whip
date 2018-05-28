using System.IO;
using System.Xml.Linq;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class TrackXmlProvider : IXmlProvider
    {
        private const string Filename = "library.xml";

        private readonly ILibrarySettings _settings;

        public TrackXmlProvider(ILibrarySettings settings)
        {
            _settings = settings;
        }

        private string XmlFilePath => Path.Combine(_settings.DataDirectory, Filename);

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
