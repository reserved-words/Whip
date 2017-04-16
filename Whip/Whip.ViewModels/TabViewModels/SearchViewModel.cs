using Whip.Common;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class SearchViewModel : TabViewModelBase
    {
        public SearchViewModel()
            :base(TabType.Search, IconType.Search, "Library Search")
        {

        }
    }
}
