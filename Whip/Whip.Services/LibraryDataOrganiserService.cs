using System.Collections.Generic;
using System.Linq;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class LibraryDataOrganiserService : ILibraryDataOrganiserService
    {
        private readonly ITaggingService _taggingService;

        public LibraryDataOrganiserService(ITaggingService taggingService)
        {
            _taggingService = taggingService;
        }

        public void AddTrack(string filepath, File file, ICollection<Artist> artists)
        {
            var id3TrackData = _taggingService.GetId3Data(filepath);

            var track = new Track
            {
                RelativeFilepath = file.RelativePath,
                Title = id3TrackData.Title,
                TrackNo = id3TrackData.TrackNo
            };

            var artist = artists.SingleOrDefault(a => a.Name == id3TrackData.Artist);

            if (artist == null)
            {
                artist = new Artist
                {
                    Name = id3TrackData.Artist
                };
                artists.Add(artist);
            }

            var albumArtist = artists.SingleOrDefault(a => a.Name == id3TrackData.AlbumArtist);

            if (albumArtist == null)
            {
                albumArtist = new Artist
                {
                    Name = id3TrackData.AlbumArtist
                };
                artists.Add(albumArtist);
            }

            var album = albumArtist.Albums.SingleOrDefault(a => a.Title == id3TrackData.AlbumTitle);

            if (album == null)
            {
                album = new Album
                {
                    Artist = albumArtist,
                    Title = id3TrackData.AlbumTitle,
                    Year = id3TrackData.AlbumYear
                };
                albumArtist.Albums.Add(album);
            }

            var disc = album.Discs.SingleOrDefault(d => d.DiscNo == id3TrackData.DiscNo);

            if (disc == null)
            {
                disc = new Disc
                {
                    Album = album,
                    DiscNo = id3TrackData.DiscNo
                };
                album.Discs.Add(disc);
            }

            disc.Tracks.Add(track);
            artist.Tracks.Add(track);

            track.Disc = disc;
            track.Artist = artist;
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
                            if (!filepathsToKeep.Contains(track.RelativeFilepath))
                            {
                                tracksToRemove.Add(track);
                            }
                            else
                            {
                                filepathsToKeep.Remove(track.RelativeFilepath);
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
    }
}
