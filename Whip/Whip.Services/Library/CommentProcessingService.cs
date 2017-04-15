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
        private const char TagDelimiter = '|';

        public string GenerateComment(Track track)
        {
            throw new NotImplementedException();
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
            }
                
            if (string.IsNullOrEmpty(artist.Wikipedia))
            {
                artist.Wikipedia = GetWikipediaString(artist.Name);
            }

            if (string.IsNullOrEmpty(artist.LastFm))
            {
                artist.LastFm = GetLastFmString(artist.Name);
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


        private string GetLastFmString(string str)
        {
            return str.Replace(" ", "+").Replace("'", "%27");
        }

        private string GetWikipediaString(string str)
        {
            return str.Replace(" ", "_").Replace("&", "%26").Replace("'", "%27");
        }
    }
}
