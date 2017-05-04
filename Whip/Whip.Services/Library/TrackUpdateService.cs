using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Whip.Common.Model;
using Whip.Common.TagModel;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class TrackUpdateService : ITrackUpdateService
    {
        private readonly ICommentProcessingService _commentProcessingService;
        private readonly ILibraryDataOrganiserService _libraryOrganiserService;
        private readonly ITaggingService _taggingService;
        private readonly ILibrarySortingService _sortingService;

        public TrackUpdateService(ILibraryDataOrganiserService libraryOrganiserService, ITaggingService taggingService,
            ICommentProcessingService commentProcessingService, ILibrarySortingService sortingService)
        {
            _commentProcessingService = commentProcessingService;
            _libraryOrganiserService = libraryOrganiserService;
            _sortingService = sortingService;
            _taggingService = taggingService;
        }

        public void SaveTrackChanges(Track trackChanged, Artist originalArtist, Disc originalDisc, bool updateTrackDetails, bool updateArtistDetails, bool updateDiscDetails, bool updateAlbumDetails)
        {
            _libraryOrganiserService.UpdateLibrary(trackChanged, originalArtist, originalDisc);

            var tracksAffected = GetTracksAffected(trackChanged, updateArtistDetails, updateDiscDetails, updateAlbumDetails);

            var trackId3Data = new Lazy<TrackId3Data>(() => GetId3Data(trackChanged));
            var artistId3Data = new Lazy<ArtistId3Data>(() => GetId3Data(trackChanged.Artist));
            var albumId3Data = new Lazy<AlbumId3Data>(() => GetId3Data(trackChanged.Disc.Album));
            var discId3Data = new Lazy<DiscId3Data>(() => GetId3Data(trackChanged.Disc));
            
            foreach (var track in tracksAffected)
            {
                var id3Data = new Id3Data
                {
                    Track = updateTrackDetails && track == trackChanged ? trackId3Data.Value : null,
                    Artist = updateArtistDetails && track.Artist == trackChanged.Artist ? artistId3Data.Value : null,
                    Album = updateAlbumDetails && track.Disc.Album == trackChanged.Disc.Album ? albumId3Data.Value : null,
                    Disc = updateDiscDetails && track.Disc == trackChanged.Disc ? discId3Data.Value : null,
                    Comment = (updateTrackDetails || updateArtistDetails) && track.Artist == trackChanged.Artist ? GetId3Comment(track) : null
                };

                _taggingService.SaveId3Data(track.File.FullPath, id3Data);

                var fileInfo = new FileInfo(track.File.FullPath);
                track.File.DateModified = fileInfo.LastWriteTime;
            }
        }

        private IEnumerable<Track> GetTracksAffected(Track trackChanged, bool updateArtistDetails, bool updateDiscDetails, bool updateAlbumDetails)
        {
            var tracks = new List<Track>()
            {
                trackChanged
            };
            
            if (updateArtistDetails)
            {
                tracks.AddRange(trackChanged.Artist.Tracks);
            }

            if (updateAlbumDetails)
            {
                tracks.AddRange(trackChanged.Disc.Album.Discs.SelectMany(d => d.Tracks));
            }
            else if (updateDiscDetails)
            {
                tracks.AddRange(trackChanged.Disc.Tracks);
            }

            return tracks.Distinct();
        }

        private string GetId3Comment(Track track)
        {
            return _commentProcessingService.GenerateComment(track);
        }

        private TrackId3Data GetId3Data(Track track)
        {
            return new TrackId3Data
            {
                Title = track.Title,
                TrackNo = track.TrackNo,
                Lyrics = track.Lyrics,
                Duration = track.Duration
            };
        }

        private ArtistId3Data GetId3Data(Artist artist)
        {
            return new ArtistId3Data
            {
                Name = artist.Name,
                SortName = artist.Sort,
                Genre = artist.Genre,
                Grouping = artist.Grouping
            };
        }

        private AlbumId3Data GetId3Data(Album album)
        {
            return new AlbumId3Data
            {
                Artist = album.Artist.Name,
                ArtistSort = album.Artist.Sort,
                Title = album.Title,
                SortTitle = _sortingService.SortValue(album),
                Year = album.Year,
                DiscCount = album.DiscCount,
                ReleaseType = album.ReleaseType.ToString(),
                Artwork = album.Artwork
            };
        }

        private DiscId3Data GetId3Data(Disc disc)
        {
            return new DiscId3Data
            {
                TrackCount = disc.TrackCount,
                DiscNo = disc.DiscNo
            };
        }
    }
}
