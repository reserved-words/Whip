using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using static Whip.Resources.Resources;

namespace Whip.Services
{
    public class XmlDataPersistenceService : IDataPersistenceService
    {
        private readonly IUserSettingsService _userSettingsService;

        public XmlDataPersistenceService(IUserSettingsService userSettingsService)
        {
            _userSettingsService = userSettingsService;
        }

        private string XmlDirectory => Path.Combine(_userSettingsService.MusicDirectory, string.Format("_{0}", ApplicationTitle));

        private string XmlFilePath => Path.Combine(XmlDirectory, "library.xml");

        public void Save(ICollection<Artist> artists)
        {
            var xml = new XDocument();

            var rootXml = new XElement(XmlPropertyNames.Root);
            xml.Add(rootXml);

            var dateUpdatedXml = new XElement(XmlPropertyNames.LastUpdated, DateTime.Now.ToString(StandardDateFormat));
            rootXml.Add(dateUpdatedXml);

            var artistsXml = new XElement(XmlPropertyNames.Artists);
            rootXml.Add(artistsXml);

            foreach (var artist in artists)
            {
                var artistXml = GetXElement(artist);
                var albumsXml = new XElement(XmlPropertyNames.Albums);
                foreach (var album in artist.Albums)
                {
                    var albumXml = GetXElement(album);
                    var discsXml = new XElement(XmlPropertyNames.Discs);
                    foreach (var disc in album.Discs)
                    {
                        var discXml = GetXElement(disc);
                        var tracksXml = new XElement(XmlPropertyNames.Tracks);
                        foreach (var track in disc.Tracks)
                        {
                            var trackXml = GetXElement(track);
                            tracksXml.Add(trackXml);
                        }
                        discXml.Add(tracksXml);
                        discsXml.Add(discXml);
                    }
                    albumXml.Add(discsXml);
                    albumsXml.Add(albumXml);
                }
                artistXml.Add(albumsXml);
                artistsXml.Add(artistXml);
            }

            Directory.CreateDirectory(XmlDirectory);

            xml.Save(XmlFilePath);
        }

        private XElement GetXElement(Artist artist)
        {
            var xml = new XElement(XmlPropertyNames.Artist);

            xml.Add(new XAttribute(XmlPropertyNames.Name, artist.Name));

            return xml;
        }

        private XElement GetXElement(Album album)
        {
            var xml = new XElement(XmlPropertyNames.Album);

            xml.Add(new XAttribute(XmlPropertyNames.Title, album.Title));
            xml.Add(new XAttribute(XmlPropertyNames.Year, album.Year));

            return xml;
        }

        private XElement GetXElement(Disc disc)
        {
            var xml = new XElement(XmlPropertyNames.Disc);

            xml.Add(new XAttribute(XmlPropertyNames.DiscNo, disc.DiscNo));

            return xml;
        }

        private XElement GetXElement(Track track)
        {
            var xml = new XElement(XmlPropertyNames.Track);

            xml.Add(new XAttribute(XmlPropertyNames.RelativeFilepath, track.RelativeFilepath));
            xml.Add(new XAttribute(XmlPropertyNames.Title, track.Title));
            xml.Add(new XAttribute(XmlPropertyNames.TrackNo, track.TrackNo));

            return xml;
        }
    }
}
