using Whip.Common;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class DashboardViewModel : TabViewModelBase
    {
        public DashboardViewModel()
            :base(TabType.Dashboard, IconType.Home, "Dashboard")
        {

        }
    }
}
