using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Whip.Common;
using Whip.Common.Model;
using Whip.Common.Model.Playlists.Criteria;
using Whip.Services.Interfaces;
using static Whip.XmlDataAccess.PropertyNames;

namespace Whip.XmlDataAccess
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private const string Filename = "playlists.xml";

        private readonly ITrackSearchService _trackSearchService;
        private readonly IUserSettings _userSettings;

        public PlaylistRepository(IUserSettings userSettings, ITrackSearchService trackSearchService)
        {
            _trackSearchService = trackSearchService;
            _userSettings = userSettings;
        }

        private string XmlFilePath => Path.Combine(_userSettings.DataDirectory, Filename);

        public AllPlaylists GetPlaylists()
        {
            var criteriaPlaylists = new List<CriteriaPlaylist>();
            var orderedPlaylists = new List<OrderedPlaylist>();
            
            if (System.IO.File.Exists(XmlFilePath))
            {
                var xml = XDocument.Load(XmlFilePath);

                foreach (var playlistXml in xml.Root.Element(PlaylistsOrdered).Elements(Playlist))
                {
                    var id = int.Parse(playlistXml.Attribute(PlaylistId).Value);
                    var title = playlistXml.Attribute(PlaylistTitle).Value;

                    var playlist = new OrderedPlaylist(id, title);

                    foreach (var trackXml in playlistXml.Element(PlaylistTracks).Elements(PlaylistTrack))
                    {
                        playlist.Tracks.Add(trackXml.Attribute(PlaylistTrackFilepath).Value);
                    }

                    orderedPlaylists.Add(playlist);
                }

                foreach (var playlistXml in xml.Root.Element(PlaylistsCriteria).Elements(Playlist))
                {
                    var id = int.Parse(playlistXml.Attribute(PlaylistId).Value);
                    var title = playlistXml.Attribute(PlaylistTitle).Value;

                    var playlist = new CriteriaPlaylist(id, title);

                    var orderByProperty = playlistXml.Attribute(PlaylistOrderBy).Value;

                    playlist.OrderByProperty = string.IsNullOrEmpty(orderByProperty)
                        ? (PropertyName?)null
                        : (PropertyName)Enum.Parse(typeof(PropertyName), orderByProperty);
                    
                    playlist.OrderByDescending = playlistXml.Attribute(PlaylistOrderByDescending).Value == TrueValue;

                    var maxTracks = playlistXml.Attribute(PlaylistMaxTracks).Value;

                    playlist.MaxTracks = string.IsNullOrEmpty(maxTracks)
                        ? (int?)null
                        : int.Parse(maxTracks);

                    playlist.CriteriaGroups = new List<CriteriaGroup>();

                    foreach (var criteriaGroupXml in playlistXml.Element(PlaylistCriteriaGroups).Elements(PlaylistCriteriaGroup))
                    {
                        var criteriaGroup = new CriteriaGroup();

                        foreach (var criteriaXml in criteriaGroupXml.Element(PlaylistArtistCriteria).Elements(PlaylistCriteria))
                        {
                            criteriaGroup.ArtistCriteria.Add(GetArtistCriteria(criteriaXml));
                        }

                        foreach (var criteriaXml in criteriaGroupXml.Element(PlaylistAlbumCriteria).Elements(PlaylistCriteria))
                        {
                            criteriaGroup.AlbumCriteria.Add(GetAlbumCriteria(criteriaXml));
                        }

                        foreach (var criteriaXml in criteriaGroupXml.Element(PlaylistDiscCriteria).Elements(PlaylistCriteria))
                        {
                            criteriaGroup.DiscCriteria.Add(GetDiscCriteria(criteriaXml));
                        }

                        foreach (var criteriaXml in criteriaGroupXml.Element(PlaylistTrackCriteria).Elements(PlaylistCriteria))
                        {
                            criteriaGroup.TrackCriteria.Add(GetTrackCriteria(criteriaXml));
                        }

                        if (criteriaGroup.TrackCriteria.Any()
                            || criteriaGroup.DiscCriteria.Any()
                            || criteriaGroup.AlbumCriteria.Any()
                            || criteriaGroup.ArtistCriteria.Any())
                        {
                            playlist.CriteriaGroups.Add(criteriaGroup);
                        }
                    }

                    criteriaPlaylists.Add(playlist);
                }
            }
            
            return new AllPlaylists
            {
                CriteriaPlaylists = criteriaPlaylists,
                OrderedPlaylists = orderedPlaylists
            };
        }

        public void Save(CriteriaPlaylist playlist)
        {
            var xml = System.IO.File.Exists(XmlFilePath)
                ? XDocument.Load(XmlFilePath)
                : CreateXmlDocument();

            var criteriaPlaylistsXml = xml.Root.Element(PlaylistsCriteria);
            
            XElement playlistXml;

            if (playlist.Id == 0)
            {
                var playlists = criteriaPlaylistsXml.Elements(Playlist);

                var maxId = playlists.Any()
                    ? playlists.Max(pl => Convert.ToInt16(pl.Attribute(PlaylistId).Value))
                    : 0;

                playlist.Id = maxId + 1;

                playlistXml = new XElement(Playlist);
                criteriaPlaylistsXml.Add(playlistXml);
            }
            else
            {
                playlistXml = criteriaPlaylistsXml
                    .Elements(Playlist)
                    .Single(pl => pl.Attribute(PlaylistId).Value == playlist.Id.ToString());
            }
            
            playlistXml.RemoveAll();

            playlistXml.Add(new XAttribute(PlaylistId, playlist.Id));
            playlistXml.Add(new XAttribute(PlaylistTitle, playlist.Title));
            playlistXml.Add(new XAttribute(PlaylistOrderBy, playlist.OrderByProperty?.ToString() ?? ""));
            playlistXml.Add(new XAttribute(PlaylistOrderByDescending, playlist.OrderByDescending ? TrueValue : FalseValue));
            playlistXml.Add(new XAttribute(PlaylistMaxTracks, playlist.MaxTracks?.ToString() ?? ""));

            var criteriaGroupsXml = new XElement(PlaylistCriteriaGroups);
            playlistXml.Add(criteriaGroupsXml);

            foreach (var criteriaGroup in playlist.CriteriaGroups)
            {
                var criteriaGroupXml = new XElement(PlaylistCriteriaGroup);

                criteriaGroupXml.Add(GetCriteriaXml(criteriaGroup.ArtistCriteria, PlaylistArtistCriteria));
                criteriaGroupXml.Add(GetCriteriaXml(criteriaGroup.AlbumCriteria, PlaylistAlbumCriteria));
                criteriaGroupXml.Add(GetCriteriaXml(criteriaGroup.DiscCriteria, PlaylistDiscCriteria));
                criteriaGroupXml.Add(GetCriteriaXml(criteriaGroup.TrackCriteria, PlaylistTrackCriteria));

                criteriaGroupsXml.Add(criteriaGroupXml);
            }

            Directory.CreateDirectory(_userSettings.DataDirectory);

            xml.Save(XmlFilePath);
        }

        public void Save(OrderedPlaylist playlist)
        {
            var xml = System.IO.File.Exists(XmlFilePath)
                ? XDocument.Load(XmlFilePath)
                : CreateXmlDocument();

            var orderedPlaylistsXml = xml.Root.Element(PlaylistsOrdered);
            
            XElement playlistXml;

            if (playlist.Id == 0)
            {
                var playlists = orderedPlaylistsXml
                    .Elements(Playlist);

                var maxId = playlists.Any()
                    ? playlists.Max(pl => Convert.ToInt16(pl.Attribute(PlaylistId).Value))
                    : 0;

                playlist.Id = maxId + 1;

                playlistXml = new XElement(Playlist);
                orderedPlaylistsXml.Add(playlistXml);
            }
            else
            {
                playlistXml = orderedPlaylistsXml
                    .Elements(Playlist)
                    .Single(pl => pl.Attribute(PlaylistId).Value == playlist.Id.ToString());
            }

            playlistXml.RemoveAll();

            playlistXml.Add(new XAttribute(PlaylistId, playlist.Id));
            playlistXml.Add(new XAttribute(PlaylistTitle, playlist.Title));

            var tracksXml = new XElement(PlaylistTracks);
            playlistXml.Add(tracksXml);

            foreach (var track in playlist.Tracks)
            {
                var trackXml = new XElement(PlaylistTrack);
                trackXml.Add(new XAttribute(PlaylistTrackFilepath, track));
                tracksXml.Add(trackXml);
            }
            
            Directory.CreateDirectory(_userSettings.DataDirectory);

            xml.Save(XmlFilePath);
        }

        private XDocument CreateXmlDocument()
        {
            var xml = new XDocument();
            var root = new XElement(PlaylistsRoot);
            root.Add(new XElement(PlaylistsCriteria));
            root.Add(new XElement(PlaylistsOrdered));
            xml.Add(root);
            return xml;
        }

        private XElement GetCriteriaXml<T>(List<Criteria<T>> criteria, string criteriaType)
        {
            var criteriaXml = new XElement(criteriaType);

            foreach (var clause in criteria)
            {
                criteriaXml.Add(GetCriteriaXml(clause));
            }

            return criteriaXml;
        }

        private XElement GetCriteriaXml<T>(Criteria<T> criteria)
        {
            var criteriaXml = new XElement(PlaylistCriteria);

            criteriaXml.Add(new XAttribute(PlaylistCriteriaPropertyName, criteria.PropertyName));
            criteriaXml.Add(new XAttribute(PlaylistCriteriaType, criteria.CriteriaType));
            criteriaXml.Add(new XAttribute(PlaylistCriteriaValue, criteria.ValueString));

            return criteriaXml;
        }

        private Criteria<Artist> GetArtistCriteria(XElement xml)
        {
            var propertyName = (PropertyName)Enum.Parse(typeof(PropertyName), xml.Attribute(PlaylistCriteriaPropertyName).Value);
            var criteriaType = (CriteriaType)Enum.Parse(typeof(CriteriaType), xml.Attribute(PlaylistCriteriaType).Value);
            var valueString = xml.Attribute(PlaylistCriteriaValue).Value;

            return _trackSearchService.GetArtistCriteria(propertyName, criteriaType, valueString);
        }

        private Criteria<Track> GetTrackCriteria(XElement xml)
        {
            var propertyName = (PropertyName)Enum.Parse(typeof(PropertyName), xml.Attribute(PlaylistCriteriaPropertyName).Value);
            var criteriaType = (CriteriaType)Enum.Parse(typeof(CriteriaType), xml.Attribute(PlaylistCriteriaType).Value);
            var valueString = xml.Attribute(PlaylistCriteriaValue).Value;

            return _trackSearchService.GetTrackCriteria(propertyName, criteriaType, valueString);
        }

        private Criteria<Disc> GetDiscCriteria(XElement xml)
        {
            var propertyName = (PropertyName)Enum.Parse(typeof(PropertyName), xml.Attribute(PlaylistCriteriaPropertyName).Value);
            var criteriaType = (CriteriaType)Enum.Parse(typeof(CriteriaType), xml.Attribute(PlaylistCriteriaType).Value);
            var valueString = xml.Attribute(PlaylistCriteriaValue).Value;

            return _trackSearchService.GetDiscCriteria(propertyName, criteriaType, valueString);
        }

        private Criteria<Album> GetAlbumCriteria(XElement xml)
        {
            var propertyName = (PropertyName)Enum.Parse(typeof(PropertyName), xml.Attribute(PlaylistCriteriaPropertyName).Value);
            var criteriaType = (CriteriaType)Enum.Parse(typeof(CriteriaType), xml.Attribute(PlaylistCriteriaType).Value);
            var valueString = xml.Attribute(PlaylistCriteriaValue).Value;

            return _trackSearchService.GetAlbumCriteria(propertyName, criteriaType, valueString);
        }

        public void Delete(CriteriaPlaylist playlist)
        {
            var xml = XDocument.Load(XmlFilePath);

            var criteriaPlaylistsXml = xml.Root.Element(PlaylistsCriteria);

            var playlistXml = criteriaPlaylistsXml
                .Elements(Playlist)
                .Single(pl => pl.Attribute(PlaylistId).Value == playlist.Id.ToString());

            playlistXml.Remove();

            xml.Save(XmlFilePath);
        }

        public void Delete(OrderedPlaylist playlist)
        {
            var xml = XDocument.Load(XmlFilePath);

            var orderedPlaylistsXml = xml.Root.Element(PlaylistsOrdered);

            var playlistXml = orderedPlaylistsXml
                .Elements(Playlist)
                .Single(pl => pl.Attribute(PlaylistId).Value == playlist.Id.ToString());

            playlistXml.Remove();

            xml.Save(XmlFilePath);
        }

        public bool ValidatePlaylistTitle(string title, int id)
        {
            if (!System.IO.File.Exists(XmlFilePath))
                return true;

            var xml = XDocument.Load(XmlFilePath);

            return xml.Root.Element(PlaylistsCriteria).Elements(Playlist)
                .All(el => el.Attribute(PlaylistTitle).Value != title
                    || el.Attribute(PlaylistId).Value == id.ToString());
        }
    }
}
