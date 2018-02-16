using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services.Utilities
{
    public class ScrobbleCacher : IScrobbleCacher
    {
        private const string RootElement = "scrobbles";
        private const string ScrobbleElement = "scrobble";
        private const string TimePlayed = "time_played";
        private const string Title = "title";
        private const string Artist = "artist";
        private const string Album = "album";
        private const string AlbumArtist = "album_artist";
        private const string Error = "error";
        private const string DateFormat = "dd/MM/yyyy HH:mm:ss";

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
            var xml = GetDocument();

            xml.Root.Add(GetScrobbleElement(track, timePlayed, "Failed once - unknown error"));

            _xmlFileService.Save(xml, XmlFilePath);
        }

        public List<Tuple<Track, DateTime>> GetCachedScrobbles()
        {
            var xml = GetDocument();

            return xml.Root.Elements(ScrobbleElement).Select(GetTuple).ToList();
        }

        public void ReplaceCache(List<Tuple<Track, DateTime, string>> scrobbles)
        {
            var xml = GetDocument();

            xml.Root.RemoveAll();

            foreach (var scrobble in scrobbles)
            {
                xml.Root.Add(GetScrobbleElement(scrobble.Item1, scrobble.Item2, scrobble.Item3));
            }

            _xmlFileService.Save(xml, XmlFilePath);
        }

        private static Tuple<Track, DateTime> GetTuple(XElement el)
        {
            var track = new Track
            {
                Title = el.Attribute(Title)?.Value,
                Artist = new Artist { Name = el.Attribute(Artist)?.Value },
                Disc = new Disc
                {
                    Album = new Album
                    {
                        Title = el.Attribute(Album)?.Value,
                        Artist = new Artist
                        {
                            Name = el.Attribute(AlbumArtist)?.Value
                        }
                    }
                }
            };

            var timePlayed = DateTime.ParseExact(el.Attribute(TimePlayed)?.Value, DateFormat, CultureInfo.CurrentCulture);

            return new Tuple<Track, DateTime>(track, timePlayed);
        }

        private XDocument GetDocument()
        {
            var xml = _xmlFileService.Get(XmlFilePath) ?? new XDocument();

            if (xml.Root == null)
            {
                xml.Add(new XElement(RootElement));
            }

            return xml;
        }

        private static XElement GetScrobbleElement(Track track, DateTime timePlayed, string error)
        {
            var el = new XElement(ScrobbleElement);
            el.Add(new XAttribute(TimePlayed, timePlayed.ToString(DateFormat)));
            el.Add(new XAttribute(Title, track.Title));
            el.Add(new XAttribute(Artist, track.Artist.Name));
            el.Add(new XAttribute(Album, track.Disc.Album.Title));
            el.Add(new XAttribute(AlbumArtist, track.Disc.Album.Artist.Name));
            el.Add(new XAttribute(Error, error));
            return el;
        }
    }
}
