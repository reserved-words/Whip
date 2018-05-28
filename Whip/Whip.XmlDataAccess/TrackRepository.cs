using System;
using System.Linq;
using System.Xml.Linq;
using Whip.Common.ExtensionMethods;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using Whip.XmlDataAccess.Interfaces;
using static Whip.Common.Resources;

namespace Whip.XmlDataAccess
{
    public class TrackRepository : ITrackRepository
    {
        private readonly ITrackXmlParser _trackXmlParser;
        private readonly IXmlProvider _xmlProvider;

        public TrackRepository(ITrackXmlParser trackXmlParser, IXmlProvider xmlProvider)
        {
            _trackXmlParser = trackXmlParser;
            _xmlProvider = xmlProvider;
        }

        public Library GetLibrary()
        {
            var xml = _xmlProvider.Get();

            if (xml == null)
            {
                return new Library
                {
                    LastUpdated = DateTime.MinValue
                };
            }

            var rootXml = xml.Root;

            var dateUpdatedXml = rootXml.Element(PropertyNames.LastUpdated);

            var library = new Library
            {
                LastUpdated = DateTime.Parse(dateUpdatedXml.Value)
            };

            var artistsXml = rootXml.Element(PropertyNames.Artists);

            foreach (var artistXml in artistsXml.Elements(PropertyNames.Artist))
            {
                var artist = _trackXmlParser.GetArtist(artistXml);

                var eventsXml = artistXml.Element(PropertyNames.Events);
                artist.UpcomingEventsUpdated = eventsXml.GetDateTimeAttribute(PropertyNames.EventsUpdated);
                foreach (var eventXml in eventsXml.Elements(PropertyNames.Event))
                {
                    artist.UpcomingEvents.Add(_trackXmlParser.GetEvent(eventXml));
                }

                library.Artists.Add(artist);
            }

            foreach (var artistXml in artistsXml.Elements(PropertyNames.Artist))
            {
                var albumArtist = library.Artists.Single(a => a.Name == artistXml.Attribute(PropertyNames.Name).Value);

                var albumsXml = artistXml.Element(PropertyNames.Albums);

                foreach (var albumXml in albumsXml.Elements(PropertyNames.Album))
                {
                    var album = _trackXmlParser.GetAlbum(albumXml, albumArtist);

                    var discsXml = albumXml.Element(PropertyNames.Discs);

                    foreach (var discXml in discsXml.Elements(PropertyNames.Disc))
                    {
                        var disc = _trackXmlParser.GetDisc(discXml, album);

                        var tracksXml = discXml.Element(PropertyNames.Tracks);

                        foreach (var trackXml in tracksXml.Elements(PropertyNames.Track))
                        {
                            var artistNameXml = trackXml.Attribute(PropertyNames.Artist);

                            var artist = artistNameXml == null
                                ? albumArtist
                                : library.Artists.Single(a => a.Name == trackXml.Attribute(PropertyNames.Artist).Value);

                            var track = _trackXmlParser.GetTrack(trackXml, disc, artist);

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
                var artistXml = _trackXmlParser.GetXElement(artist);

                var albumsXml = new XElement(PropertyNames.Albums);
                foreach (var album in artist.Albums)
                {
                    var albumXml = _trackXmlParser.GetXElement(album);
                    var discsXml = new XElement(PropertyNames.Discs);
                    foreach (var disc in album.Discs)
                    {
                        var discXml = _trackXmlParser.GetXElement(disc);
                        var tracksXml = new XElement(PropertyNames.Tracks);
                        foreach (var track in disc.Tracks)
                        {
                            var trackXml = _trackXmlParser.GetXElement(track);
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
            
            _xmlProvider.Save(xml);
        }
    }
}