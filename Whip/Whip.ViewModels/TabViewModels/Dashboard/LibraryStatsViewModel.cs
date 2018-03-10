using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Whip.Common.Model;
using System.Linq;
using Whip.Common.ExtensionMethods;
using Whip.Services.Interfaces;

namespace Whip.ViewModels.TabViewModels.Dashboard
{
    public class LibraryStatsViewModel : ViewModelBase
    {
        private readonly ILibraryStatisticsService _service;

        public LibraryStatsViewModel(ILibraryStatisticsService service)
        {
            _service = service;

            GeneralStatistics = new ObservableCollection<Statistic>();
            TracksByArtist = new ObservableCollection<Statistic>();
            AlbumsByReleaseType = new ObservableCollection<Statistic>();
            ArtistsByGrouping = new ObservableCollection<Statistic>();
        }

        public ObservableCollection<Statistic> GeneralStatistics { get; }
        public ObservableCollection<Statistic> TracksByArtist { get; }
        public ObservableCollection<Statistic> AlbumsByReleaseType { get; }
        public ObservableCollection<Statistic> ArtistsByGrouping { get; }

        public void Refresh()
        {
            GeneralStatistics.Clear();
            TracksByArtist.Clear();
            AlbumsByReleaseType.Clear();
            ArtistsByGrouping.Clear();

            GeneralStatistics.Add(new Statistic("Track Artists", _service.GetNumberOfTrackArtists()));
            GeneralStatistics.Add(new Statistic("Album Artists", _service.GetNumberOfAlbumArtists()));
            GeneralStatistics.Add(new Statistic("Albums", _service.GetNumberOfAlbums()));
            GeneralStatistics.Add(new Statistic("Tracks", _service.GetNumberOfTracks()));
            GeneralStatistics.Add(new Statistic("Total Time", _service.GetTotalTime()));

            _service.GetTracksByArtist(10)
                .Select(a => new Statistic(a.Item1, a.Item2))
                .ToList()
                .ForEach(TracksByArtist.Add);

            _service.GetAlbumsByReleaseType()
                .Select(a => new Statistic(a.Item1.GetDisplayName(), a.Item2))
                .ToList()
                .ForEach(AlbumsByReleaseType.Add);

            _service.GetArtistsByGrouping()
                .Select(a => new Statistic(a.Item1, a.Item2))
                .ToList()
                .ForEach(ArtistsByGrouping.Add);
        }
    }
}
