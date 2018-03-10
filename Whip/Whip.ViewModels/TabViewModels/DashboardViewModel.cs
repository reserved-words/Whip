using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.ViewModels.TabViewModels.Dashboard;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class DashboardViewModel : TabViewModelBase
    {
        public DashboardViewModel(ILibraryStatisticsService libraryStatisticsService, Common.Singletons.Library library,
            IRecentTracksService recentTracksService, IUserSettings userSettings)
            :base(TabType.Dashboard, IconType.Home, "Home")
        {
            LibraryStatsViewModel = new LibraryStatsViewModel(libraryStatisticsService);
            RecentTracksViewModel = new RecentTracksViewModel(recentTracksService, userSettings);

            library.Updated += Library_Updated;
        }

        private void Library_Updated(Track trackUpdated)
        {
            if (IsVisible)
            {
                LibraryStatsViewModel.Refresh();
            }
        }

        public LibraryStatsViewModel LibraryStatsViewModel { get; }
        public RecentTracksViewModel RecentTracksViewModel { get; }

        public override void OnShow(Track currentTrack)
        {
            LibraryStatsViewModel.Refresh();
            RecentTracksViewModel.Refresh();
        }
    }
}
