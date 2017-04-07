using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace TagLibSharp
{
    public class TagLibService : ITaggingService
    {
        private static Dictionary<string, string> exceptionalCorrections = new Dictionary<string, string> { { "AC; DC", "AC/DC" } };

        public TrackId3Data GetId3Data(string filepath)
        {
            using (TagLib.File track = TagLib.File.Create(filepath))
            {
                TagLib.Id3v2.Tag.DefaultVersion = 3;
                TagLib.Id3v2.Tag.ForceDefaultVersion = true;

                return new TrackId3Data
                {
                    Title = track.Tag.Title,
                    Artist = exceptionalCorrections.Keys.Contains(track.Tag.JoinedPerformers)
                        ? exceptionalCorrections[track.Tag.JoinedPerformers]
                        : track.Tag.FirstPerformer,
                    AlbumArtist = exceptionalCorrections.Keys.Contains(track.Tag.JoinedAlbumArtists)
                        ? exceptionalCorrections[track.Tag.JoinedAlbumArtists]
                        : track.Tag.FirstAlbumArtist,
                    AlbumTitle = track.Tag.Album,
                    AlbumYear = track.Tag.Year.ToString(),
                    TrackNo = (int)track.Tag.Track,
                    TrackCount = (int)track.Tag.TrackCount,
                    DiscNo = (int)track.Tag.Disc,
                    DiscCount = (int)track.Tag.DiscCount,
                    Genre = track.Tag.Genres.FirstOrDefault(),
                    Grouping = track.Tag.Grouping,
                    Duration = track.Properties.Duration,
                    Comment = track.Tag.Comment,
                    Lyrics = track.Tag.Lyrics
                };
            }
        }
    }
}
