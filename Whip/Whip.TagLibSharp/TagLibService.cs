using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagLib;
using Whip.Common.TagModel;
using Whip.Services.Interfaces;

namespace Whip.TagLibSharp
{
    public class TagLibService : ITaggingService
    {
        private static Dictionary<string, string> exceptionalCorrections = new Dictionary<string, string> { { "AC; DC", "AC/DC" } };

        public BasicTrackId3Data GetBasicId3Data(string filepath)
        {
            using (TagLib.File f = TagLib.File.Create(filepath))
            {
                TagLib.Id3v2.Tag.DefaultVersion = 3;
                TagLib.Id3v2.Tag.ForceDefaultVersion = true;

                return new BasicTrackId3Data
                {
                    Title = GetValue(f.Tag.Title),
                    Duration = f.Properties.Duration,
                    ArtistName = GetArtistName(f.Tag.JoinedPerformers, f.Tag.FirstPerformer),
                    AlbumArtistName = GetArtistName(f.Tag.JoinedAlbumArtists, f.Tag.FirstAlbumArtist),
                    AlbumTitle = GetValue(f.Tag.Album),
                    Year = f.Tag.Year.ToString(),
                    TrackNo = (int)f.Tag.Track,
                    DiscNo = (int)f.Tag.Disc
                };
            }
        }

        public Id3Data GetTrackId3Data(string filepath)
        {
            using (TagLib.File f = TagLib.File.Create(filepath))
            {
                TagLib.Id3v2.Tag.DefaultVersion = 3;
                TagLib.Id3v2.Tag.ForceDefaultVersion = true;

                var track = new TrackId3Data
                {
                    Title = GetValue(f.Tag.Title),
                    TrackNo = (int)f.Tag.Track,
                    Duration = f.Properties.Duration,
                    Lyrics = GetValue(f.Tag.Lyrics),
                };

                var artist = new ArtistId3Data
                {
                    Name = GetArtistName(f.Tag.JoinedPerformers, f.Tag.FirstPerformer),
                    Genre = GetValue(f.Tag.Genres.FirstOrDefault()),
                    Grouping = GetValue(f.Tag.Grouping)
                };

                var album = new AlbumId3Data
                {
                    Artist = GetArtistName(f.Tag.JoinedAlbumArtists, f.Tag.FirstAlbumArtist),
                    Title = GetValue(f.Tag.Album),
                    Year = f.Tag.Year.ToString(),
                    DiscCount = (int)f.Tag.DiscCount,
                    ReleaseType = GetValue(f.Tag.MusicBrainzReleaseType),
                    Artwork = f.Tag.Pictures.Count() > 0 ? f.Tag.Pictures.First().Data.Data : null
                };

                var disc = new DiscId3Data
                {
                    TrackCount = (int)f.Tag.TrackCount,
                    DiscNo = (int)f.Tag.Disc
                };
                
                return new Id3Data
                {
                    Track = track,
                    Artist = artist,
                    Album = album,
                    Disc = disc,
                    Comment = f.Tag.Comment
                };
            }
        }

        public void SaveId3Data(string filepath, Id3Data data)
        {
            using (TagLib.File f = TagLib.File.Create(filepath))
            {
                TagLib.Id3v2.Tag.DefaultVersion = 3;
                TagLib.Id3v2.Tag.ForceDefaultVersion = true;

                if (data.Track != null)
                {
                    f.Tag.Title = data.Track.Title;
                    f.Tag.Lyrics = data.Track.Lyrics;
                    f.Tag.Track = Convert.ToUInt16(data.Track.TrackNo);
                }

                if (data.Artist != null)
                {
                    f.Tag.Performers = null;
                    f.Tag.PerformersSort = null;
                    f.Tag.Performers = new[] { data.Artist.Name };
                    f.Tag.PerformersSort = new[] { data.Artist.SortName };
                    f.Tag.Genres = null;
                    f.Tag.Genres = new[] { data.Artist.Genre };
                    f.Tag.Grouping = data.Artist.Grouping;
                }

                if (data.Album != null)
                {
                    f.Tag.Album = data.Album.Title;
                    f.Tag.AlbumSort = data.Album.SortTitle;

                    f.Tag.AlbumArtists = null;
                    f.Tag.AlbumArtistsSort = null;
                    f.Tag.AlbumArtists = new[] { data.Album.Artist };
                    f.Tag.AlbumArtistsSort = new[] { data.Album.ArtistSort };

                    f.Tag.Year = Convert.ToUInt16(data.Album.Year);

                    f.Tag.MusicBrainzReleaseType = data.Album.ReleaseType;

                    f.Tag.DiscCount = Convert.ToUInt16(data.Album.DiscCount);

                    f.Tag.Pictures = null;
                    if (data.Album.Artwork != null)
                    {
                        f.Tag.Pictures = new IPicture[] { CreatePicture(data.Album.Artwork) };
                    }
                }

                if (data.Disc != null)
                {
                    f.Tag.TrackCount = Convert.ToUInt16(data.Disc.TrackCount);
                    f.Tag.Disc = Convert.ToUInt16(data.Disc.DiscNo);
                }

                if (data.Comment != null)
                {
                    f.Tag.Comment = data.Comment;
                }

                f.Save();
            }
        }

        private Picture CreatePicture(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return new Picture()
                {
                    Data = ByteVector.FromStream(ms),
                    Type = PictureType.FrontCover,
                    MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg,
                    Description = "Cover",
                };
            }
        }

        private string GetValue(string str)
        {
            return str ?? string.Empty;
        }

        private string GetArtistName(string joinedArtists, string firstArtist)
        {
            return exceptionalCorrections.Keys.Contains(joinedArtists)
                ? exceptionalCorrections[joinedArtists]
                : GetValue(firstArtist);
        }
    }
}
