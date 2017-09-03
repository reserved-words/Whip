﻿using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.ExtensionMethods;
using Whip.Common.Model;
using Whip.Common.TrackSorters;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.Services
{
    public class Playlist : IPlaylist
    {
        private readonly TrackQueue _queue = new TrackQueue();
        private readonly IUserSettings _userSettings;
        private readonly ILoggingService _logger;

        public Playlist(IUserSettings userSettings, ILoggingService logger)
        {
            _userSettings = userSettings;
            _logger = logger;
        }

        public event Action ListUpdated;
        public event Action<Track> CurrentTrackChanged;

        public Track CurrentTrack => _queue.CurrentTrack;

        public string PlaylistName { get; private set; }

        public List<Track> Tracks { get; private set; }

        public void Set(string playlistName, List<Track> tracks, Track startAt)
        {
            _logger.Info($"Playlist Set: {playlistName} ({tracks.Count} tracks)");

            PlaylistName = playlistName;
            Tracks = tracks;

            var sortedTracks = Sort(tracks);

            _queue.Set(sortedTracks, startAt == null ? 0 : sortedTracks.IndexOf(startAt), true);

            _logger.Info("Queue has been set");

            ListUpdated?.Invoke();
        }

        public void MoveNext()
        {
            _logger.Info("Playlist move next called");
            _queue.MoveNext();
            _logger.Info("Calling current track changed for " + (_queue.CurrentTrack?.Title ?? "null track"));
            CurrentTrackChanged?.Invoke(_queue.CurrentTrack);
        }

        public void MovePrevious()
        {
            _queue.MovePrevious();
            CurrentTrackChanged?.Invoke(_queue.CurrentTrack);
        }

        private List<Track> Sort(List<Track> tracks)
        {
            if (_userSettings.ShuffleOn)
            {
                return tracks.SortUsing(new RandomTrackSorter()).ToList();
            }
            else
            {
                return tracks.SortUsing(new DefaultTrackSorter()).ToList();
            }
        }

        public void Reorder()
        {
            var sortedTracks = Sort(Tracks);

            _queue.Set(sortedTracks, CurrentTrack == null ? 0 : sortedTracks.IndexOf(CurrentTrack) + 1, false);

            ListUpdated?.Invoke();
        }
    }
}
