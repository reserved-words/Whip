using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Whip.Common;

namespace Whip.ViewModels.TabViewModels.Dashboard
{
    public class LibraryStatsViewModel : ViewModelBase
    {
        public LibraryStatsViewModel()
        {
            GeneralStatistics = new List<Statistic>
            {
                new Statistic("Track Artists", 150),
                new Statistic("Album Artists", 100),
                new Statistic("Albums", 1000),
                new Statistic("Tracks", 10000),
                new Statistic("Total Time", new TimeSpan(5, 5, 2, 16))
            };
            AlbumsByReleaseType = new List<Statistic>
            {
                new Statistic(ReleaseType.Album.GetReleaseTypeGroupingDisplayName(), 381),
                new Statistic(ReleaseType.Single.GetReleaseTypeGroupingDisplayName(), 15),
                new Statistic(ReleaseType.BestOf.GetReleaseTypeGroupingDisplayName(), 21)
            };
            ArtistsByGrouping = new List<Statistic>
            {
                new Statistic("Metal", 38),
                new Statistic("Pop", 35),
                new Statistic("Alternative", 21)
            };
        }

        public List<Statistic> GeneralStatistics { get; set; }
        public List<Statistic> AlbumsByReleaseType { get; set; }
        public List<Statistic> ArtistsByGrouping { get; set; }

        public class Statistic
        {
            public Statistic(string caption, object data)
            {
                Caption = caption;
                Data = data;
            }

            public string Caption { get; }
            public object Data { get; }
        }
    }
}
