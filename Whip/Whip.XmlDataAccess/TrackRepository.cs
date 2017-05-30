using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Whip.Common;
using Whip.Common.ExtensionMethods;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Common.Utilities;
using Whip.Services.Interfaces;
using static Whip.Resources.Resources;

namespace Whip.XmlDataAccess
{
    public class TrackRepository : ITrackRepository
    {
        private const char TagsDelimiter = '|';

        private readonly IUserSettings _userSettings;

        public TrackRepository(IUserSettings userSettings)
        {
            _userSettings = userSettings;
        }

        private string XmlFilePath => Path.Combine(_userSettings.DataDirectory, "library.xml");

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

            var dateUpdatedXml = rootXml.Element(PropertyNames.LastUpdated);

            var library = new Library
            {
                LastUpdated = DateTime.Parse(dateUpdatedXml.Value)
            };

            var artistsXml = rootXml.Element(PropertyNames.Artists);

            foreach (var artistXml in artistsXml.Elements(PropertyNames.Artist))
            {
                var artist = GetArtist(artistXml);

                var eventsXml = artistXml.Element(PropertyNames.Events);
                artist.UpcomingEventsUpdated = eventsXml.GetDateTimeAttribute(PropertyNames.EventsUpdated);
                foreach (var eventXml in eventsXml.Elements(PropertyNames.Event))
                {
                    artist.UpcomingEvents.Add(GetEvent(eventXml));
                }

                library.Artists.Add(artist);
            }

            foreach (var artistXml in artistsXml.Elements(PropertyNames.Artist))
            {
                var albumArtist = library.Artists.Single(a => a.Name == artistXml.Attribute(PropertyNames.Name).Value);

                var albumsXml = artistXml.Element(PropertyNames.Albums);

                foreach (var albumXml in albumsXml.Elements(PropertyNames.Album))
                {
                    var album = GetAlbum(albumXml, albumArtist);

                    var discsXml = albumXml.Element(PropertyNames.Discs);

                    foreach (var discXml in discsXml.Elements(PropertyNames.Disc))
                    {
                        var disc = GetDisc(discXml, album);

                        var tracksXml = discXml.Element(PropertyNames.Tracks);

                        foreach (var trackXml in tracksXml.Elements(PropertyNames.Track))
                        {
                            var artistNameXml = trackXml.Attribute(PropertyNames.Artist);

                            var artist = artistNameXml == null
                                ? albumArtist
                                : library.Artists.Single(a => a.Name == trackXml.Attribute(PropertyNames.Artist).Value);

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

            var rootXml = new XElement(PropertyNames.Root);
            xml.Add(rootXml);

            var dateUpdatedXml = new XElement(PropertyNames.LastUpdated, library.LastUpdated.ToString(StandardDateFormat));
            rootXml.Add(dateUpdatedXml);

            var artistsXml = new XElement(PropertyNames.Artists);
            rootXml.Add(artistsXml);

            foreach (var artist in library.Artists)
            {
                var artistXml = GetXElement(artist);

                var eventsXml = new XElement(PropertyNames.Events);
                eventsXml.Add(new XAttribute(PropertyNames.EventsUpdated, artist.UpcomingEventsUpdated.ToString(StandardDateFormat)));
                foreach (var ev in artist.UpcomingEvents)
                {
                    var eventXml = GetXElement(ev);
                    eventsXml.Add(eventXml);
                }
                artistXml.Add(eventsXml);

                var albumsXml = new XElement(PropertyNames.Albums);
                foreach (var album in artist.Albums)
                {
                    var albumXml = GetXElement(album);
                    var discsXml = new XElement(PropertyNames.Discs);
                    foreach (var disc in album.Discs)
                    {
                        var discXml = GetXElement(disc);
                        var tracksXml = new XElement(PropertyNames.Tracks);
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

            Directory.CreateDirectory(_userSettings.DataDirectory);

            xml.Save(XmlFilePath);
        }

        private Artist GetArtist(XElement xml)
        {
            return new Artist
            {
                Name = xml.GetAttribute(PropertyNames.Name),
                Genre = xml.GetAttribute(PropertyNames.Genre),
                Grouping = xml.GetAttribute(PropertyNames.Grouping),
                Website = xml.GetAttribute(PropertyNames.Website),
                Twitter = xml.GetAttribute(PropertyNames.Twitter),
                Facebook = xml.GetAttribute(PropertyNames.Facebook),
                LastFm = xml.GetAttribute(PropertyNames.LastFm),
                Wikipedia = xml.GetAttribute(PropertyNames.Wikipedia),
                YouTube = xml.GetAttribute(PropertyNames.YouTube),
                BandCamp = xml.GetAttribute(PropertyNames.BandCamp),
                BandsInTown = xml.GetAttribute(PropertyNames.BandsInTown),
                City = new City(xml.GetAttribute(PropertyNames.City), xml.GetAttribute(PropertyNames.State), xml.GetAttribute(PropertyNames.Country))
            };
        }

        private Album GetAlbum(XElement xml, Artist artist)
        {
            return new Album
            {
                Artist = artist,
                Title = xml.GetAttribute(PropertyNames.Title),
                Year = xml.GetAttribute(PropertyNames.Year),
                DiscCount = xml.GetIntAttribute(PropertyNames.DiscCount),
                ReleaseType = EnumHelpers.Parse<ReleaseType>(xml.GetAttribute(PropertyNames.ReleaseType))
            };
        }

        private Disc GetDisc(XElement xml, Album album)
        {
            return new Disc
            {
                Album = album,
                DiscNo = xml.GetIntAttribute(PropertyNames.DiscNo),
                TrackCount = xml.GetIntAttribute(PropertyNames.TrackCount)
            };
        }

        private Track GetTrack(XElement xml, Disc disc, Artist artist)
        {
            return new Track
            {
                Disc = disc,
                Artist = artist,
                Title = xml.GetAttribute(PropertyNames.Title),
                File = new Common.Model.File(
                    xml.GetAttribute(PropertyNames.FullFilepath),
                    xml.GetAttribute(PropertyNames.RelativeFilepath),
                    xml.GetDateTimeAttribute(PropertyNames.DateCreated),
                    xml.GetDateTimeAttribute(PropertyNames.DateModified)
                ),
                TrackNo = xml.GetIntAttribute(PropertyNames.TrackNo),
                Duration = TimeSpan.ParseExact(xml.GetAttribute(PropertyNames.Duration), StandardTimeSpanFormat, CultureInfo.InvariantCulture),
                Tags = xml.GetAttribute(PropertyNames.Tags).Split(TagsDelimiter).ToList(),
                Year = xml.GetAttribute(PropertyNames.TrackYear),
                Lyrics = xml.GetAttribute(PropertyNames.Lyrics)
            };
        }

        private ArtistEvent GetEvent(XElement xml)
        {
            return new ArtistEvent
            {
                Date = xml.GetDateTimeAttribute(PropertyNames.EventDate),
                Venue = xml.GetAttribute(PropertyNames.EventVenue),
                City = xml.GetAttribute(PropertyNames.EventCity),
                Country = xml.GetAttribute(PropertyNames.EventCountry),
                ArtistList = xml.GetAttribute(PropertyNames.EventArtistList),
            };
        }

        private XElement GetXElement(Artist artist)
        {
            var xml = new XElement(PropertyNames.Artist);

            xml.AddAttribute(PropertyNames.Name, artist.Name);
            xml.AddAttribute(PropertyNames.Genre, artist.Genre);
            xml.AddAttribute(PropertyNames.Grouping, artist.Grouping);
            xml.AddAttribute(PropertyNames.Website, artist.Website);
            xml.AddAttribute(PropertyNames.Twitter, artist.Twitter);
            xml.AddAttribute(PropertyNames.Facebook, artist.Facebook);
            xml.AddAttribute(PropertyNames.LastFm, artist.LastFm);
            xml.AddAttribute(PropertyNames.Wikipedia, artist.Wikipedia);
            xml.AddAttribute(PropertyNames.YouTube, artist.YouTube);
            xml.AddAttribute(PropertyNames.BandCamp, artist.BandCamp);
            xml.AddAttribute(PropertyNames.BandsInTown, artist.BandsInTown);
            xml.AddAttribute(PropertyNames.City, artist.City?.Name);
            xml.AddAttribute(PropertyNames.State, artist.City?.State);
            xml.AddAttribute(PropertyNames.Country, artist.City?.Country);

            return xml;
        }

        private XElement GetXElement(Album album)
        {
            var xml = new XElement(PropertyNames.Album);

            xml.AddAttribute(PropertyNames.Title, album.Title);
            xml.AddAttribute(PropertyNames.Year, album.Year);
            xml.AddAttribute(PropertyNames.DiscCount, album.DiscCount);
            xml.AddAttribute(PropertyNames.ReleaseType, album.ReleaseType.ToString());

            return xml;
        }

        private XElement GetXElement(Disc disc)
        {
            var xml = new XElement(PropertyNames.Disc);

            xml.AddAttribute(PropertyNames.DiscNo, disc.DiscNo);
            xml.AddAttribute(PropertyNames.TrackCount, disc.TrackCount);

            return xml;
        }

        private XElement GetXElement(Track track)
        {
            var xml = new XElement(PropertyNames.Track);

            xml.AddAttribute(PropertyNames.RelativeFilepath, track.File.RelativePath);
            xml.AddAttribute(PropertyNames.FullFilepath, track.File.FullPath);
            xml.AddAttribute(PropertyNames.DateCreated, track.File.DateCreated.ToString(StandardDateFormat));
            xml.AddAttribute(PropertyNames.DateModified, track.File.DateModified.ToString(StandardDateFormat));
            xml.AddAttribute(PropertyNames.Title, track.Title);
            xml.AddAttribute(PropertyNames.TrackNo, track.TrackNo);
            xml.AddAttribute(PropertyNames.Duration, track.Duration.ToString(StandardTimeSpanFormat));
            xml.AddAttribute(PropertyNames.TrackYear, track.Year);
            xml.AddAttribute(PropertyNames.Tags, string.Join(TagsDelimiter.ToString(), track.Tags));
            xml.AddAttribute(PropertyNames.Lyrics, track.Lyrics);

            if (track.Artist != track.Disc.Album.Artist)
            {
                xml.AddAttribute(PropertyNames.Artist, track.Artist.Name);
            }

            return xml;
        }

        private XElement GetXElement(ArtistEvent ev)
        {
            var xml = new XElement(PropertyNames.Event);

            xml.AddAttribute(PropertyNames.EventDate, ev.Date.ToString(StandardDateFormat));
            xml.AddAttribute(PropertyNames.EventVenue, ev.Venue);
            xml.AddAttribute(PropertyNames.EventCity, ev.City);
            xml.AddAttribute(PropertyNames.EventCountry, ev.Country);
            xml.AddAttribute(PropertyNames.EventArtistList, ev.ArtistList);

            return xml;
        }
    }
}