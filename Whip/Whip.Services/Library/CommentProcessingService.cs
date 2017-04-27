using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Whip.Common.ExtensionMethods;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class CommentProcessingService : ICommentProcessingService
    {
        private const string Root = "info";
        private const string TrackYear = "track_year";
        private const string Country = "country";
        private const string State = "state";
        private const string City = "city";
        private const string Tags = "tags";
        private const string Website = "website";
        private const string Twitter = "twitter";
        private const string Facebook = "facebook";
        private const string LastFm = "lastfm";
        private const string Wikipedia = "wikipedia";
        private const string YouTube = "youtube";
        private const string BandsInTown = "bands_in_town";
        private const string BandCamp = "bandcamp";

        private const char TagDelimiter = '|';

        public string GenerateComment(Track track)
        {
            var xml = new XDocument();

            var root = new XElement(Root);
            xml.Add(root);

            root.Add(new XElement(TrackYear, track.Year));
            root.Add(new XElement(Tags, string.Join(TagDelimiter.ToString(), track.Tags.Where(t => !string.IsNullOrEmpty(t)))));
            root.Add(new XElement(Country, track.Artist.City.Country));
            root.Add(new XElement(State, track.Artist.City.State));
            root.Add(new XElement(City, track.Artist.City.Name));
            root.Add(new XElement(Website, track.Artist.Website));
            root.Add(new XElement(Twitter, track.Artist.Twitter));
            root.Add(new XElement(Facebook, track.Artist.Facebook));
            root.Add(new XElement(LastFm, track.Artist.LastFm));
            root.Add(new XElement(Wikipedia, track.Artist.Wikipedia));
            root.Add(new XElement(YouTube, track.Artist.Wikipedia));
            root.Add(new XElement(BandsInTown, track.Artist.Wikipedia));
            root.Add(new XElement(BandCamp, track.Artist.Wikipedia));

            return xml.ToString();
        }

        public void Populate(Track track, string comment)
        {
            XDocument xml;
            if (TryParseXml(comment, out xml))
            {
                track.Year = xml.Root.GetValue(TrackYear);
                track.Tags = xml.Root.GetValue(Tags)
                    .Split(new char[] { TagDelimiter }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList() ?? new List<string>();
            }

            if (string.IsNullOrEmpty(track.Year))
            {
                track.Year = track.Disc.Album.Year;
            }
        }

        public void Populate(Artist artist, string comment)
        {
            XDocument xml;
            if (TryParseXml(comment, out xml))
            {
                artist.City = new City(xml.Root.GetValue(City), xml.Root.GetValue(State), xml.Root.GetValue(Country));
                artist.Website = xml.Root.GetValue(Website);
                artist.Twitter = xml.Root.GetValue(Twitter);
                artist.Facebook = xml.Root.GetValue(Facebook);
                artist.LastFm = xml.Root.GetValue(LastFm);
                artist.Wikipedia = xml.Root.GetValue(Wikipedia);
                artist.YouTube = xml.Root.GetValue(YouTube);
                artist.BandCamp = xml.Root.GetValue(BandCamp);
                artist.BandsInTown = xml.Root.GetValue(BandsInTown);
            }

            if (string.IsNullOrEmpty(artist.BandsInTown))
            {
                artist.BandsInTown = GetBandsInTownIdentifier(artist.Name);
            }

            if (string.IsNullOrEmpty(artist.Wikipedia))
            {
                artist.Wikipedia = GetWikipediaIdentifier(artist.Name);
            }

            if (string.IsNullOrEmpty(artist.LastFm))
            {
                artist.LastFm = GetLastFmIdentifier(artist.Name);
            }
        }

        private bool TryParseXml(string xmlString, out XDocument xml)
        {
            try
            {
                xml = XDocument.Parse(xmlString);
                return true;
            }
            catch (Exception)
            {
                xml = null;
                return false;
            }
        }

        private string GetBandsInTownIdentifier(string str)
        {
            return str.Replace(" ", "");
        }

        private string GetLastFmIdentifier(string str)
        {
            return str.Replace(" ", "+").Replace("'", "%27");
        }

        private string GetWikipediaIdentifier(string str)
        {
            return str.Replace(" ", "_").Replace("&", "%26").Replace("'", "%27");
        }
    }
}
