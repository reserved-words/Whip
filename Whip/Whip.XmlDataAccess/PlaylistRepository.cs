using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Whip.Common;
using Whip.Common.Enums;
using Whip.Common.Model;
using Whip.Common.Model.Playlists.Criteria;
using Whip.Services.Interfaces;
using static Whip.XmlDataAccess.PropertyNames;

namespace Whip.XmlDataAccess
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private const string Filename = "playlists.xml";
        private const char ValueSeparator = '|';

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
            var quickPlaylists = new List<QuickPlaylist>();
            
            if (System.IO.File.Exists(XmlFilePath))
            {
                var xml = XDocument.Load(XmlFilePath);

                if (xml.Root.Element(PlaylistsOrdered) != null)
                {
                    foreach (var playlistXml in xml.Root.Element(PlaylistsOrdered).Elements(PropertyNames.Playlist))
                    {
                        orderedPlaylists.Add(CreateOrderedPlaylist(playlistXml));
                    } 
                }

                if (xml.Root.Element(PlaylistsCriteria) != null)
                {
                    foreach (var playlistXml in xml.Root.Element(PlaylistsCriteria).Elements(PropertyNames.Playlist))
                    {
                        criteriaPlaylists.Add(CreateCriteriaPlaylist(playlistXml));
                    } 
                }

                if (xml.Root.Element(PlaylistsFavouriteQuick) != null)
                {
                    foreach (var playlistXml in xml.Root.Element(PlaylistsFavouriteQuick).Elements(PropertyNames.Playlist))
                    {
                        quickPlaylists.Add(CreateQuickPlaylist(playlistXml));
                    } 
                }
            }
            
            return new AllPlaylists
            {
                CriteriaPlaylists = criteriaPlaylists,
                OrderedPlaylists = orderedPlaylists,
                FavouriteQuickPlaylists = quickPlaylists
            };
        }

        public List<Playlist> GetFavouritePlaylists()
        {
            var playlists = new List<Playlist>();

            if (System.IO.File.Exists(XmlFilePath))
            {
                var xml = XDocument.Load(XmlFilePath);

                if (xml.Root.Element(PlaylistsOrdered) != null)
                {
                    foreach (var playlistXml in xml.Root.Element(PlaylistsOrdered).Elements(PropertyNames.Playlist)
                        .Where(pl => pl.Attribute(PlaylistIsFavourite)?.Value == TrueValue))
                    {
                        playlists.Add(CreatePlaylist(playlistXml, PlaylistType.Ordered));
                    }
                }

                if (xml.Root.Element(PlaylistsCriteria) != null)
                {
                    foreach (var playlistXml in xml.Root.Element(PlaylistsCriteria).Elements(PropertyNames.Playlist)
                        .Where(pl => pl.Attribute(PlaylistIsFavourite)?.Value == TrueValue))
                    {
                        playlists.Add(CreatePlaylist(playlistXml, PlaylistType.Criteria));
                    }
                }

                if (xml.Root.Element(PlaylistsFavouriteQuick) != null)
                {
                    foreach (var playlistXml in xml.Root.Element(PlaylistsFavouriteQuick).Elements(PropertyNames.Playlist))
                    {
                        playlists.Add(CreatePlaylist(playlistXml, PlaylistType.Quick));
                    }
                }
            }

            return playlists;
        }

        private static Playlist CreatePlaylist(XElement playlistXml, PlaylistType type)
        {
            var id = int.Parse(playlistXml.Attribute(PlaylistId).Value);
            var title = playlistXml.Attribute(PlaylistTitle).Value;
            var favourite = type == PlaylistType.Quick || playlistXml.Attribute(PlaylistIsFavourite)?.Value == TrueValue;
            return new Playlist(type, id, title, favourite);
        }

        private QuickPlaylist CreateQuickPlaylist(XElement playlistXml)
        {
            var id = int.Parse(playlistXml.Attribute(PlaylistId).Value);
            var title = playlistXml.Attribute(PlaylistTitle).Value;
            var filterType = (FilterType)Enum.Parse(typeof(FilterType), playlistXml.Attribute(PlaylistFilter).Value);
            var filterValues = playlistXml.Attribute(PlaylistFilterValue).Value.Split(ValueSeparator);

            var playlist = new QuickPlaylist(id, title, true, filterType, filterValues);

            return playlist;
        }

        private OrderedPlaylist CreateOrderedPlaylist(XElement playlistXml)
        {
            var id = int.Parse(playlistXml.Attribute(PlaylistId).Value);
            var title = playlistXml.Attribute(PlaylistTitle).Value;
            var favourite = playlistXml.Attribute(PlaylistIsFavourite)?.Value == TrueValue;

            var playlist = new OrderedPlaylist(id, title, favourite);

            foreach (var trackXml in playlistXml.Element(PlaylistTracks).Elements(PlaylistTrack))
            {
                playlist.Tracks.Add(trackXml.Attribute(PlaylistTrackFilepath).Value);
            }

            return playlist;
        }

        private CriteriaPlaylist CreateCriteriaPlaylist(XElement playlistXml)
        {
            var id = int.Parse(playlistXml.Attribute(PlaylistId).Value);
            var title = playlistXml.Attribute(PlaylistTitle).Value;
            var favourite = playlistXml.Attribute(PlaylistIsFavourite)?.Value == TrueValue;

            var playlist = new CriteriaPlaylist(id, title, favourite);

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

            return playlist;
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
                var playlists = criteriaPlaylistsXml.Elements(PropertyNames.Playlist);

                var maxId = playlists.Any()
                    ? playlists.Max(pl => Convert.ToInt16(pl.Attribute(PlaylistId).Value))
                    : 0;

                playlist.Id = maxId + 1;

                playlistXml = new XElement(PropertyNames.Playlist);
                criteriaPlaylistsXml.Add(playlistXml);
            }
            else
            {
                playlistXml = criteriaPlaylistsXml
                    .Elements(PropertyNames.Playlist)
                    .Single(pl => pl.Attribute(PlaylistId).Value == playlist.Id.ToString());
            }
            
            playlistXml.RemoveAll();

            playlistXml.Add(new XAttribute(PlaylistId, playlist.Id));
            playlistXml.Add(new XAttribute(PlaylistTitle, playlist.Title));
            playlistXml.Add(new XAttribute(PlaylistIsFavourite, playlist.Favourite ? TrueValue : FalseValue));
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

        public void Save(QuickPlaylist playlist)
        {
            var xml = System.IO.File.Exists(XmlFilePath)
                ? XDocument.Load(XmlFilePath)
                : CreateXmlDocument();

            var favouriteQuickPlaylistsXml = xml.Root.Element(PlaylistsFavouriteQuick);

            if (favouriteQuickPlaylistsXml == null)
            {
                favouriteQuickPlaylistsXml = new XElement(PlaylistsFavouriteQuick);
                xml.Root.Add(favouriteQuickPlaylistsXml);
            }

            XElement playlistXml;

            if (!playlist.Favourite)
            {
                playlistXml = favouriteQuickPlaylistsXml
                    .Elements(PropertyNames.Playlist)
                    .Single(pl => pl.Attribute(PlaylistId).Value == playlist.Id.ToString());

                playlistXml.Remove();
            }
            else
            {
                var playlists = favouriteQuickPlaylistsXml
                    .Elements(PropertyNames.Playlist);

                var maxId = playlists.Any()
                    ? playlists.Max(pl => Convert.ToInt16(pl.Attribute(PlaylistId).Value))
                    : 0;

                playlist.Id = maxId + 1;

                playlistXml = new XElement(PropertyNames.Playlist);
                playlistXml.Add(new XAttribute(PlaylistId, playlist.Id));
                playlistXml.Add(new XAttribute(PlaylistTitle, playlist.GetDefaultTitle()));
                playlistXml.Add(new XAttribute(PlaylistFilter, playlist.FilterType.ToString()));
                playlistXml.Add(new XAttribute(PlaylistFilterValue, string.Join(ValueSeparator.ToString(), playlist.FilterValues)));

                favouriteQuickPlaylistsXml.Add(playlistXml);
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
                    .Elements(PropertyNames.Playlist);

                var maxId = playlists.Any()
                    ? playlists.Max(pl => Convert.ToInt16(pl.Attribute(PlaylistId).Value))
                    : 0;

                playlist.Id = maxId + 1;

                playlistXml = new XElement(PropertyNames.Playlist);
                orderedPlaylistsXml.Add(playlistXml);
            }
            else
            {
                playlistXml = orderedPlaylistsXml
                    .Elements(PropertyNames.Playlist)
                    .Single(pl => pl.Attribute(PlaylistId).Value == playlist.Id.ToString());
            }

            playlistXml.RemoveAll();

            playlistXml.Add(new XAttribute(PlaylistId, playlist.Id));
            playlistXml.Add(new XAttribute(PlaylistTitle, playlist.Title));
            playlistXml.Add(new XAttribute(PlaylistIsFavourite, playlist.Favourite ? TrueValue : FalseValue));

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
            root.Add(new XElement(PlaylistsFavouriteQuick));
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
            criteriaXml.Add(new XAttribute(PlaylistCriteriaValue, criteria.ValueString ?? ""));

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
                .Elements(PropertyNames.Playlist)
                .Single(pl => pl.Attribute(PlaylistId).Value == playlist.Id.ToString());

            playlistXml.Remove();

            xml.Save(XmlFilePath);
        }

        public void Delete(OrderedPlaylist playlist)
        {
            var xml = XDocument.Load(XmlFilePath);

            var orderedPlaylistsXml = xml.Root.Element(PlaylistsOrdered);

            var playlistXml = orderedPlaylistsXml
                .Elements(PropertyNames.Playlist)
                .Single(pl => pl.Attribute(PlaylistId).Value == playlist.Id.ToString());

            playlistXml.Remove();

            xml.Save(XmlFilePath);
        }

        public CriteriaPlaylist GetCriteriaPlaylist(int id)
        {
            if (!System.IO.File.Exists(XmlFilePath))
                throw new ApplicationException("No playlists created");

            var xml = XDocument.Load(XmlFilePath);

            var criteriaPlaylists = xml.Root.Element(PlaylistsCriteria).Elements(PropertyNames.Playlist);

            var playlistXml = criteriaPlaylists.SingleOrDefault(x => x.Attribute(PlaylistId).Value == id.ToString());

            if (playlistXml == null)
                throw new ApplicationException($"Requested criteria playlist ID {id} does not exist");

            return CreateCriteriaPlaylist(playlistXml);
        }

        public OrderedPlaylist GetOrderedPlaylist(int id)
        {
            if (!System.IO.File.Exists(XmlFilePath))
                throw new ApplicationException("No playlists created");

            var xml = XDocument.Load(XmlFilePath);

            var orderedPlaylists = xml.Root.Element(PlaylistsOrdered).Elements(PropertyNames.Playlist);

            var playlistXml = orderedPlaylists.SingleOrDefault(x => x.Attribute(PlaylistId).Value == id.ToString());

            if (playlistXml == null)
                throw new ApplicationException($"Requested ordered playlist ID {id} does not exist");

            return CreateOrderedPlaylist(playlistXml);
        }

        public QuickPlaylist GetQuickPlaylist(int id)
        {
            if (!System.IO.File.Exists(XmlFilePath))
                throw new ApplicationException("No playlists created");

            var xml = XDocument.Load(XmlFilePath);

            var quickPlaylists = xml.Root.Element(PlaylistsFavouriteQuick).Elements(PropertyNames.Playlist);

            var playlistXml = quickPlaylists.SingleOrDefault(x => x.Attribute(PlaylistId).Value == id.ToString());

            if (playlistXml == null)
                throw new ApplicationException($"Requested quick playlist ID {id} does not exist");

            return CreateQuickPlaylist(playlistXml);
        }

        public List<OrderedPlaylist> GetOrderedPlaylists()
        {
            var orderedPlaylists = new List<OrderedPlaylist>();
            
            if (System.IO.File.Exists(XmlFilePath))
            {
                var xml = XDocument.Load(XmlFilePath);

                if (xml.Root.Element(PlaylistsOrdered) != null)
                {
                    foreach (var playlistXml in xml.Root.Element(PlaylistsOrdered).Elements(PropertyNames.Playlist))
                    {
                        orderedPlaylists.Add(CreateOrderedPlaylist(playlistXml));
                    }
                }
            }

            return orderedPlaylists;
        }
    }
}
