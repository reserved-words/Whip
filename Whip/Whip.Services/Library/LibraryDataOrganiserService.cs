using System.Collections.Generic;
using System.Linq;
using Whip.Common;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Common.Utilities;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class LibraryDataOrganiserService : ILibraryDataOrganiserService
    {
        private readonly Library _library;
        private readonly ITaggingService _taggingService;
        private readonly ICommentProcessingService _commentProcessingService;
        private readonly ILibrarySortingService _sortingService;

        public LibraryDataOrganiserService(ITaggingService taggingService, ICommentProcessingService commentProcessingService, Library library,
            ILibrarySortingService sortingService)
        {
            _library = library;
            _sortingService = sortingService;
            _taggingService = taggingService;
            _commentProcessingService = commentProcessingService;
        }

        public void AddTrack(string filepath, File file, ICollection<Artist> artists)
        {
            var id3Data = _taggingService.GetTrackId3Data(filepath);

            var track = new Track
            {
                File = file,
                Title = id3Data.Track.Title,
                TrackNo = id3Data.Track.TrackNo,
                Duration = id3Data.Track.Duration,
                Lyrics = id3Data.Track.Lyrics
            };

            var artist = artists.SingleOrDefault(a => a.Name == id3Data.Artist.Name);

            if (artist == null)
            {
                artist = new Artist
                {
                    Name = id3Data.Artist.Name,
                    Genre = id3Data.Artist.Genre,
                    Grouping = id3Data.Artist.Grouping
                };
                artists.Add(artist);
            }

            if (!artist.Tracks.Any())
            {
                _commentProcessingService.Populate(artist, id3Data.Track.Comment);
            }

            var albumArtist = artists.SingleOrDefault(a => a.Name == id3Data.Album.Artist);

            if (albumArtist == null)
            {
                albumArtist = new Artist
                {
                    Name = id3Data.Album.Artist
                };
                artists.Add(albumArtist);
            }

            var album = albumArtist.Albums.SingleOrDefault(a => a.Title == id3Data.Album.Title);

            if (album == null)
            {
                album = new Album
                {
                    Artist = albumArtist,
                    Title = id3Data.Album.Title,
                    Year = id3Data.Album.Year,
                    DiscCount = id3Data.Album.DiscCount,
                    ReleaseType = EnumHelpers.Parse<ReleaseType>(id3Data.Album.ReleaseType)
                };
                albumArtist.Albums.Add(album);
            }

            var disc = album.Discs.SingleOrDefault(d => d.DiscNo == id3Data.Disc.DiscNo);

            if (disc == null)
            {
                disc = new Disc
                {
                    Album = album,
                    DiscNo = id3Data.Disc.DiscNo,
                    TrackCount = id3Data.Disc.TrackCount
                };
                album.Discs.Add(disc);
            }

            disc.Tracks.Add(track);
            artist.Tracks.Add(track);

            track.Disc = disc;
            track.Artist = artist;

            _commentProcessingService.Populate(track, id3Data.Track.Comment);
        }

        public void SyncTracks(ICollection<Artist> artists, ICollection<string> filepathsToKeep)
        {
            foreach (var artist in artists)
            {
                foreach (var album in artist.Albums)
                {
                    foreach (var disc in album.Discs)
                    {
                        var tracksToRemove = new List<Track>();

                        foreach (var track in disc.Tracks)
                        {
                            if (!filepathsToKeep.Contains(track.File.RelativePath))
                            {
                                tracksToRemove.Add(track);
                            }
                            else
                            {
                                filepathsToKeep.Remove(track.File.RelativePath);
                            }
                        }

                        foreach (var track in tracksToRemove)
                        {
                            disc.Tracks.Remove(track);
                            track.Artist.Tracks.Remove(track);
                        }

                    }

                    album.Discs.Where(d => !d.Tracks.Any())
                        .ToList()
                        .ForEach(d => album.Discs.Remove(d));
                }

                artist.Albums.Where(a => !a.Discs.Any())
                    .ToList()
                    .ForEach(a => artist.Albums.Remove(a));
            }

            artists.Where(a => !a.Albums.Any() && !a.Tracks.Any())
                .ToList()
                .ForEach(a => artists.Remove(a));
        }

        public void UpdateLibrary(Track trackChanged, Artist originalArtist, Disc originalDisc)
        {
            var newArtist = trackChanged.Artist;
            var newDisc = trackChanged.Disc;

            if (newArtist != originalArtist)
            {
                originalArtist.Tracks.Remove(trackChanged);
                newArtist.Tracks.Add(trackChanged);

                if (!_library.Artists.Contains(newArtist))
                {
                    _library.Artists.Add(newArtist);
                }
            }

            if (newDisc != originalDisc)
            {
                originalDisc.Tracks.Remove(trackChanged);
                newDisc.Tracks.Add(trackChanged);

                if (!newDisc.Album.Discs.Contains(newDisc))
                {
                    newDisc.Album.Discs.Add(newDisc);
                }

                if (!newDisc.Album.Artist.Albums.Contains(newDisc.Album))
                {
                    newDisc.Album.Artist.Albums.Add(newDisc.Album);
                }

                if (!_library.Artists.Contains(newDisc.Album.Artist))
                {
                    _library.Artists.Add(newDisc.Album.Artist);
                }
            }

            if (!originalDisc.Tracks.Any())
            {
                originalDisc.Album.Discs.Remove(originalDisc);
            }

            if (!originalDisc.Album.Discs.Any())
            {
                originalDisc.Album.Artist.Albums.Remove(originalDisc.Album);
            }

            if (!originalDisc.Album.Artist.Albums.Any())
            {
                _library.Artists.Remove(originalDisc.Album.Artist);
            }

            if (!originalArtist.Tracks.Any() && !originalArtist.Albums.Any())
            {
                _library.Artists.Remove(originalArtist);
            }

            _sortingService.SortTracks(newDisc);

            _sortingService.SortDiscs(newDisc.Album);

            _library.OnTrackUpdated(trackChanged);
        }
    }
}
