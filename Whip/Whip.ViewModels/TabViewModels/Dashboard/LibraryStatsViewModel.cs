using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Whip.Common;

namespace Whip.ViewModels.TabViewModels.Dashboard
{
    public class LibraryStatsViewModel : ViewModelBase
    {
        public LibraryStatsViewModel()
        {
            NumberOfTrackArtists = 150;
            NumberOfAlbumArtists = 100;
            NumberOfAlbums = 1000;
            NumberOfTracks = 10000;
            AlbumsByReleaseType = new List < Statistic >
            {
                new Statistic(ReleaseType.Album.GetReleaseTypeGroupingDisplayName(), 381),
                new Statistic(ReleaseType.Single.GetReleaseTypeGroupingDisplayName(), 15),
                new Statistic(ReleaseType.BestOf.GetReleaseTypeGroupingDisplayName(), 21)
            };
            ArtistsByGrouping = new List< Statistic >
            {
                new Statistic("Metal", 38),
                new Statistic("Pop", 35),
                new Statistic("Alternative", 21)
            };
            TotalTime = new TimeSpan(5,5,2,16);
        }

        public int NumberOfTrackArtists { get; set; }
        public int NumberOfAlbumArtists { get; set; }
        public int NumberOfAlbums { get; set; }
        public int NumberOfTracks { get; set; }
        public List<Statistic> AlbumsByReleaseType { get; set; }
        public List<Statistic> ArtistsByGrouping { get; set; }
        public TimeSpan TotalTime { get; set; }

        public class Statistic
        {
            public Statistic(string caption, int count)
            {
                Caption = caption;
                Count = count;
            }

            public string Caption { get; }
            public int Count { get; }
        }
    }
}
