using System;
using System.IO;
using System.Xml.Linq;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services.Utilities
{
    public class ScrobbleCacher : IScrobbleCacher
    {
        private readonly IXmlFileService _xmlFileService;
        private readonly IUserSettings _userSettings;

        public ScrobbleCacher(IUserSettings userSettings, IXmlFileService xmlFileService)
        {
            _userSettings = userSettings;
            _xmlFileService = xmlFileService;
        }

        private string XmlFilePath => Path.Combine(_userSettings.DataDirectory, "scrobble-cache.xml");

        public void Cache(Track track, DateTime timePlayed)
        {
            var xml = _xmlFileService.Get(XmlFilePath) ?? new XDocument();

            if (xml.Root == null)
            {
                xml.Add(new XElement("scrobbles"));
            }

            var el = new XElement("scrobble");
            el.Add(new XAttribute("TimePlayed", timePlayed.ToString("dd/MM/yyyy HH:mm:ss")));
            el.Add(new XAttribute("Title", track.Title));
            el.Add(new XAttribute("Artist", track.Artist.Name));
            el.Add(new XAttribute("Album", track.Disc.Album.Title));
            el.Add(new XAttribute("AlbumArtist", track.Disc.Album.Artist.Name));
            xml.Root.Add(el);

            _xmlFileService.Save(xml, XmlFilePath);
        }
    }
}
