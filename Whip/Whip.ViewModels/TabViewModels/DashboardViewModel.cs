using System.Security.Cryptography.X509Certificates;
using Whip.Common;
using Whip.ViewModels.TabViewModels.Dashboard;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class DashboardViewModel : TabViewModelBase
    {
        public DashboardViewModel()
            :base(TabType.Dashboard, IconType.Home, "Dashboard")
        {
            LibraryStatsViewModel = new LibraryStatsViewModel();
        }

        public LibraryStatsViewModel LibraryStatsViewModel { get; }
    }
}
