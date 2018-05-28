using System.IO;
using System.Xml.Linq;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class PlaylistXmlProvider : IXmlProvider
    {
        private const string Filename = "playlists.xml";

        private readonly IUserSettings _userSettings;

        public PlaylistXmlProvider(IUserSettings userSettings)
        {
            _userSettings = userSettings;
        }

        private string XmlFilePath => Path.Combine(_userSettings.DataDirectory, Filename);

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
