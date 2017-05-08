using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using static Whip.XmlDataAccess.PropertyNames;

namespace Whip.XmlDataAccess
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private const string Filename = "playlists.xml";

        private readonly IUserSettings _userSettings;

        public PlaylistRepository(IUserSettings userSettings)
        {
            _userSettings = userSettings;
        }

        private string XmlFilePath => Path.Combine(_userSettings.DataDirectory, Filename);

        public AllPlaylists GetPlaylists()
        {
            var criteriaPlaylists = new List<CriteriaPlaylist>();
            var orderedPlaylists = new List<OrderedPlaylist>();

            var feeds = new List<Feed>();

            if (System.IO.File.Exists(XmlFilePath))
            {
                var xml = XDocument.Load(XmlFilePath);

                foreach (var playlistXml in xml.Root.Element(PlaylistsOrdered).Elements(Playlist))
                {
                    var playlist = new OrderedPlaylist
                    {
                        Id = int.Parse(playlistXml.Attribute(PlaylistId).Value),
                        Title = playlistXml.Attribute(PlaylistTitle).Value
                    };

                    foreach (var trackXml in playlistXml.Element(PlaylistTracks).Elements(PlaylistTrack))
                    {
                        playlist.Tracks.Add(trackXml.Attribute(PlaylistTrackFilepath).Value);
                    }

                    orderedPlaylists.Add(playlist);
                }

                foreach (var playlistXml in xml.Root.Element(PlaylistsCriteria).Elements(Playlist))
                {
                    criteriaPlaylists.Add(new CriteriaPlaylist
                    {
                        Id = int.Parse(playlistXml.Attribute(PlaylistId).Value),
                        Title = playlistXml.Attribute(PlaylistTitle).Value
                    });
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
            throw new NotImplementedException();
        }

        public void Save(OrderedPlaylist playlist)
        {
            throw new NotImplementedException();
        }
    }
}
