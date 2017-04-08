using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Whip.Common.Enums;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Common.Utilities;
using Whip.Services.Interfaces;
using static Whip.Resources.Resources;

namespace Whip.Services
{
    public class XmlDataPersistenceService : IDataPersistenceService
    {
        private const char TagsDelimiter = '|';

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
                Name = xml.GetAttribute(XmlPropertyNames.Name),
                Genre = xml.GetAttribute(XmlPropertyNames.Genre),
                Grouping = xml.GetAttribute(XmlPropertyNames.Grouping),
                Website = xml.GetAttribute(XmlPropertyNames.Website),
                Twitter = xml.GetAttribute(XmlPropertyNames.Twitter),
                Facebook = xml.GetAttribute(XmlPropertyNames.Facebook),
                City = new City(xml.GetAttribute(XmlPropertyNames.City), xml.GetAttribute(XmlPropertyNames.State), xml.GetAttribute(XmlPropertyNames.Country))
            };
        }

        private Album GetAlbum(XElement xml, Artist artist)
        {
            return new Album
            {
                Artist = artist,
                Title = xml.GetAttribute(XmlPropertyNames.Title),
                Year = xml.GetAttribute(XmlPropertyNames.Year),
                DiscCount = xml.GetIntAttribute(XmlPropertyNames.DiscCount),
                ReleaseType = EnumHelpers.Parse<ReleaseType>(xml.GetAttribute(XmlPropertyNames.ReleaseType))
            };
        }

        private Disc GetDisc(XElement xml, Album album)
        {
            return new Disc
            {
                Album = album,
                DiscNo = xml.GetIntAttribute(XmlPropertyNames.DiscNo),
                TrackCount = xml.GetIntAttribute(XmlPropertyNames.TrackCount)
            };
        }

        private Track GetTrack(XElement xml, Disc disc, Artist artist)
        {
            return new Track
            {
                Disc = disc,
                Artist = artist,
                Title = xml.GetAttribute(XmlPropertyNames.Title),
                RelativeFilepath = xml.GetAttribute(XmlPropertyNames.RelativeFilepath),
                FullFilepath = xml.GetAttribute(XmlPropertyNames.FullFilepath),
                TrackNo = xml.GetIntAttribute(XmlPropertyNames.TrackNo),
                Duration = TimeSpan.ParseExact(xml.GetAttribute(XmlPropertyNames.Duration), StandardTimeSpanFormat, CultureInfo.InvariantCulture),
                Tags = xml.GetAttribute(XmlPropertyNames.Tags).Split(TagsDelimiter).ToList(),
                Year = xml.GetAttribute(XmlPropertyNames.TrackYear),
                Lyrics = xml.GetAttribute(XmlPropertyNames.Lyrics)
            };
        }

        private XElement GetXElement(Artist artist)
        {
            var xml = new XElement(XmlPropertyNames.Artist);

            xml.AddAttribute(XmlPropertyNames.Name, artist.Name);
            xml.AddAttribute(XmlPropertyNames.Genre, artist.Genre);
            xml.AddAttribute(XmlPropertyNames.Grouping, artist.Grouping);
            xml.AddAttribute(XmlPropertyNames.Website, artist.Website);
            xml.AddAttribute(XmlPropertyNames.Twitter, artist.Twitter);
            xml.AddAttribute(XmlPropertyNames.Facebook, artist.Facebook);
            xml.AddAttribute(XmlPropertyNames.City, artist.City.Name);
            xml.AddAttribute(XmlPropertyNames.State, artist.City.State);
            xml.AddAttribute(XmlPropertyNames.Country, artist.City.Country);

            return xml;
        }

        private XElement GetXElement(Album album)
        {
            var xml = new XElement(XmlPropertyNames.Album);

            xml.AddAttribute(XmlPropertyNames.Title, album.Title);
            xml.AddAttribute(XmlPropertyNames.Year, album.Year);
            xml.AddAttribute(XmlPropertyNames.DiscCount, album.DiscCount);
            xml.AddAttribute(XmlPropertyNames.ReleaseType, album.ReleaseType.ToString());

            return xml;
        }

        private XElement GetXElement(Disc disc)
        {
            var xml = new XElement(XmlPropertyNames.Disc);

            xml.AddAttribute(XmlPropertyNames.DiscNo, disc.DiscNo);
            xml.AddAttribute(XmlPropertyNames.TrackCount, disc.TrackCount);

            return xml;
        }

        private XElement GetXElement(Track track)
        {
            var xml = new XElement(XmlPropertyNames.Track);

            xml.AddAttribute(XmlPropertyNames.RelativeFilepath, track.RelativeFilepath);
            xml.AddAttribute(XmlPropertyNames.FullFilepath, track.FullFilepath);
            xml.AddAttribute(XmlPropertyNames.Title, track.Title);
            xml.AddAttribute(XmlPropertyNames.TrackNo, track.TrackNo);
            xml.AddAttribute(XmlPropertyNames.Duration, track.Duration.ToString(StandardTimeSpanFormat));
            xml.AddAttribute(XmlPropertyNames.TrackYear, track.Year);
            xml.AddAttribute(XmlPropertyNames.Tags, string.Join(TagsDelimiter.ToString(), track.Tags));
            xml.AddAttribute(XmlPropertyNames.Lyrics, track.Lyrics);

            if (track.Artist != track.Disc.Album.Artist)
            {
                xml.AddAttribute(XmlPropertyNames.Artist, track.Artist.Name);
            }

            return xml;
        }
    }
}