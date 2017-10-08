using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Whip.Common;
using Whip.Common.ExtensionMethods;
using Whip.Common.Model;
using Whip.Common.Utilities;
using Whip.XmlDataAccess.Interfaces;
using static Whip.Common.Resources;

namespace Whip.XmlDataAccess
{
    public class TrackXmlParser : ITrackXmlParser
    {
        private const char TagsDelimiter = '|';

        public XElement GetXElement(Track track)
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

            if (!track.Artist.Equals(track.Disc.Album.Artist))
            {
                xml.AddAttribute(PropertyNames.Artist, track.Artist.Name);
            }

            return xml;
        }

        public Track GetTrack(XElement xml, Disc disc, Artist artist)
        {
            return new Track
            {
                Disc = disc,
                Artist = artist,
                Title = xml.GetAttribute(PropertyNames.Title),
                File = new File(
                    xml.GetAttribute(PropertyNames.FullFilepath),
                    xml.GetAttribute(PropertyNames.RelativeFilepath),
                    xml.GetDateTimeAttribute(PropertyNames.DateCreated),
                    xml.GetDateTimeAttribute(PropertyNames.DateModified)
                ),
                TrackNo = xml.GetIntAttribute(PropertyNames.TrackNo),
                Duration = TimeSpan.ParseExact(xml.GetAttribute(PropertyNames.Duration), StandardTimeSpanFormat, CultureInfo.InvariantCulture),
                Tags = xml.GetAttribute(PropertyNames.Tags).Split(TagsDelimiter).Where(t => !string.IsNullOrEmpty(t)).ToList(),
                Year = xml.GetAttribute(PropertyNames.TrackYear),
                Lyrics = xml.GetAttribute(PropertyNames.Lyrics)
            };
        }

        public Artist GetArtist(XElement xml)
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
                City = new City(xml.GetAttribute(PropertyNames.City), xml.GetAttribute(PropertyNames.State), xml.GetAttribute(PropertyNames.Country)),
                WebInfo = GetWebInfo(xml.Element(PropertyNames.WebInfo)),
                VideoUpdated = xml.Element(PropertyNames.Video).GetDateTimeAttribute(PropertyNames.VideoUpdated),
                LatestVideo = GetVideo(xml.Element(PropertyNames.Video))
            };
        }

        public Video GetVideo(XElement xml)
        {
            var url = xml.GetAttribute(PropertyNames.VideoUrl);

            return string.IsNullOrEmpty(url)
                ? null
                : new Video
                {
                    Url = url,
                    Title = xml.GetAttribute(PropertyNames.VideoTitle),
                    Published = xml.GetDateTimeAttribute(PropertyNames.VideoPublished)
                };
        }

        public ArtistWebInfo GetWebInfo(XElement xml)
        {
            var webInfo = new ArtistWebInfo
            {
                Updated = xml.GetDateTimeAttribute(PropertyNames.WebInfoUpdated),
                Wiki = xml.GetAttribute(PropertyNames.WebInfoWiki),
                SmallImageUrl = xml.GetAttribute(PropertyNames.WebInfoImageSmall),
                MediumImageUrl = xml.GetAttribute(PropertyNames.WebInfoImageMedium),
                LargeImageUrl = xml.GetAttribute(PropertyNames.WebInfoImageLarge),
                ExtraLargeImageUrl = xml.GetAttribute(PropertyNames.WebInfoImageExtraLarge)
            };

            foreach (var artistXml in xml.Element(PropertyNames.WebInfoSimilarArtists).Elements(PropertyNames.WebInfoSimilarArtist))
            {
                webInfo.SimilarArtists.Add(new ArtistWebSimilarArtist
                {
                    Name = artistXml.GetAttribute(PropertyNames.WebInfoSimilarArtistName),
                    Url = artistXml.GetAttribute(PropertyNames.WebInfoSimilarArtistUrl),
                    ImageUrl = artistXml.GetAttribute(PropertyNames.WebInfoSimilarArtistImage)
                });
            }

            return webInfo;
        }

        public Album GetAlbum(XElement xml, Artist artist)
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

        public Disc GetDisc(XElement xml, Album album)
        {
            return new Disc
            {
                Album = album,
                DiscNo = xml.GetIntAttribute(PropertyNames.DiscNo),
                TrackCount = xml.GetIntAttribute(PropertyNames.TrackCount)
            };
        }

        public ArtistEvent GetEvent(XElement xml)
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

        public XElement GetXElement(Artist artist)
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

            var eventsXml = new XElement(PropertyNames.Events);
            eventsXml.Add(new XAttribute(PropertyNames.EventsUpdated, artist.UpcomingEventsUpdated.ToString(StandardDateFormat)));
            foreach (var ev in artist.UpcomingEvents)
            {
                var eventXml = GetXElement(ev);
                eventsXml.Add(eventXml);
            }
            xml.Add(eventsXml);

            var videoXml = new XElement(PropertyNames.Video);
            videoXml.AddAttribute(PropertyNames.VideoUpdated, artist.VideoUpdated.ToString(StandardDateFormat));
            videoXml.AddAttribute(PropertyNames.VideoUrl, artist.LatestVideo?.Url);
            videoXml.AddAttribute(PropertyNames.VideoTitle, artist.LatestVideo?.Title);
            videoXml.AddAttribute(PropertyNames.VideoPublished, artist.LatestVideo?.Published.ToString(StandardDateFormat));
            xml.Add(videoXml);

            xml.Add(GetXElement(artist.WebInfo));

            return xml;
        }

        public XElement GetXElement(ArtistWebInfo webInfo)
        {
            var xml = new XElement(PropertyNames.WebInfo);

            xml.AddAttribute(PropertyNames.WebInfoUpdated, webInfo.Updated.ToString(StandardDateFormat));
            xml.AddAttribute(PropertyNames.WebInfoWiki, webInfo.Wiki);
            xml.AddAttribute(PropertyNames.WebInfoImageSmall, webInfo.MediumImageUrl);
            xml.AddAttribute(PropertyNames.WebInfoImageMedium, webInfo.SmallImageUrl);
            xml.AddAttribute(PropertyNames.WebInfoImageLarge, webInfo.LargeImageUrl);
            xml.AddAttribute(PropertyNames.WebInfoImageExtraLarge, webInfo.ExtraLargeImageUrl);

            var similarArtistsXml = new XElement(PropertyNames.WebInfoSimilarArtists);
            foreach (var artist in webInfo.SimilarArtists)
            {
                var artistXml = new XElement(PropertyNames.WebInfoSimilarArtist);
                artistXml.AddAttribute(PropertyNames.WebInfoSimilarArtistName, artist.Name);
                artistXml.AddAttribute(PropertyNames.WebInfoSimilarArtistUrl, artist.Url);
                artistXml.AddAttribute(PropertyNames.WebInfoSimilarArtistImage, artist.ImageUrl);
                similarArtistsXml.Add(artistXml);
            }
            xml.Add(similarArtistsXml);

            return xml;
        }

        public XElement GetXElement(Album album)
        {
            var xml = new XElement(PropertyNames.Album);

            xml.AddAttribute(PropertyNames.Title, album.Title);
            xml.AddAttribute(PropertyNames.Year, album.Year);
            xml.AddAttribute(PropertyNames.DiscCount, album.DiscCount);
            xml.AddAttribute(PropertyNames.ReleaseType, album.ReleaseType.ToString());

            return xml;
        }

        public XElement GetXElement(Disc disc)
        {
            var xml = new XElement(PropertyNames.Disc);

            xml.AddAttribute(PropertyNames.DiscNo, disc.DiscNo);
            xml.AddAttribute(PropertyNames.TrackCount, disc.TrackCount);

            return xml;
        }

        public XElement GetXElement(ArtistEvent ev)
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
