using System;
using System.Globalization;
using System.IO;
using System.Linq;
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

        public Library GetLibrary()
        {
            if (!System.IO.File.Exists(XmlFilePath))
            {
                return new Library
                {
                    LastUpdated = DateTime.MinValue
                };
            }

            var xml = XDocument.Load(XmlFilePath);

            var rootXml = xml.Root;

            var dateUpdatedXml = rootXml.Element(XmlPropertyNames.LastUpdated);

            var library = new Library
            {
                LastUpdated = DateTime.Parse(dateUpdatedXml.Value)
            };

            var artistsXml = rootXml.Element(XmlPropertyNames.Artists);

            foreach (var artistXml in artistsXml.Elements(XmlPropertyNames.Artist))
            {
                var artist = GetArtist(artistXml);

                library.Artists.Add(artist);
            }

            foreach (var artistXml in artistsXml.Elements(XmlPropertyNames.Artist))
            {
                var albumArtist = library.Artists.Single(a => a.Name == artistXml.Attribute(XmlPropertyNames.Name).Value);

                var albumsXml = artistXml.Element(XmlPropertyNames.Albums);

                foreach (var albumXml in albumsXml.Elements(XmlPropertyNames.Album))
                {
                    var album = GetAlbum(albumXml, albumArtist);

                    var discsXml = albumXml.Element(XmlPropertyNames.Discs);

                    foreach (var discXml in discsXml.Elements(XmlPropertyNames.Disc))
                    {
                        var disc = GetDisc(discXml, album);

                        var tracksXml = discXml.Element(XmlPropertyNames.Tracks);

                        foreach (var trackXml in tracksXml.Elements(XmlPropertyNames.Track))
                        {
                            var artistNameXml = trackXml.Attribute(XmlPropertyNames.Artist);

                            var artist = artistNameXml == null
                                ? albumArtist
                                : library.Artists.Single(a => a.Name == trackXml.Attribute(XmlPropertyNames.Artist).Value);

                            var track = GetTrack(trackXml, disc, artist);

                            disc.Tracks.Add(track);
                            artist.Tracks.Add(track);
                        }

                        album.Discs.Add(disc);
                    }

                    albumArtist.Albums.Add(album);
                }
            }

            return library;
        }

        public void Save(Library library)
        {
            var xml = new XDocument();

            var rootXml = new XElement(XmlPropertyNames.Root);
            xml.Add(rootXml);

            var dateUpdatedXml = new XElement(XmlPropertyNames.LastUpdated, library.LastUpdated.ToString(StandardDateFormat));
            rootXml.Add(dateUpdatedXml);

            var artistsXml = new XElement(XmlPropertyNames.Artists);
            rootXml.Add(artistsXml);

            foreach (var artist in library.Artists)
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

        private Artist GetArtist(XElement xml)
        {
            return new Artist
            {
                Name = xml.Attribute(XmlPropertyNames.Name).Value,
                Genre = xml.Attribute(XmlPropertyNames.Genre).Value
            };
        }

        private Album GetAlbum(XElement xml, Artist artist)
        {
            return new Album
            {
                Artist = artist,
                Title = xml.Attribute(XmlPropertyNames.Title).Value,
                Year = xml.Attribute(XmlPropertyNames.Year).Value
            };
        }

        private Disc GetDisc(XElement xml, Album album)
        {
            return new Disc
            {
                Album = album,
                DiscNo = Convert.ToInt16(xml.Attribute(XmlPropertyNames.DiscNo).Value)
            };
        }

        private Track GetTrack(XElement xml, Disc disc, Artist artist)
        {
            return new Track
            {
                Disc = disc,
                Artist = artist,
                Title = xml.Attribute(XmlPropertyNames.Title).Value,
                RelativeFilepath = xml.Attribute(XmlPropertyNames.RelativeFilepath).Value,
                FullFilepath = xml.Attribute(XmlPropertyNames.FullFilepath).Value,
                TrackNo = Convert.ToInt16(xml.Attribute(XmlPropertyNames.TrackNo).Value),
                Duration = TimeSpan.ParseExact(xml.Attribute(XmlPropertyNames.Duration).Value, StandardTimeSpanFormat, CultureInfo.InvariantCulture)
            };
        }

        private XElement GetXElement(Artist artist)
        {
            var xml = new XElement(XmlPropertyNames.Artist);

            xml.Add(new XAttribute(XmlPropertyNames.Name, artist.Name));
            xml.Add(new XAttribute(XmlPropertyNames.Genre, artist.Genre));

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
            xml.Add(new XAttribute(XmlPropertyNames.FullFilepath, track.FullFilepath));
            xml.Add(new XAttribute(XmlPropertyNames.Title, track.Title));
            xml.Add(new XAttribute(XmlPropertyNames.TrackNo, track.TrackNo));
            xml.Add(new XAttribute(XmlPropertyNames.Duration, track.Duration.ToString(StandardTimeSpanFormat)));

            if (track.Artist != track.Disc.Album.Artist)
            {
                xml.Add(new XAttribute(XmlPropertyNames.Artist, track.Artist.Name));
            }

            return xml;
        }
    }
}