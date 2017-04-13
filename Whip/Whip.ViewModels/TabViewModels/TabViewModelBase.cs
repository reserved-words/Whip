using GalaSoft.MvvmLight;
using Whip.Common.ExtensionMethods;
using Whip.Common.Model;
using Whip.Common.Utilities;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class TabViewModelBase : ViewModelBase
    {
        public TabViewModelBase(TabType type)
        {
            Key = type;

            var metaData = type.GetAttribute<MetaDataAttribute>();

            Title = metaData.DisplayName;
            Icon = metaData.IconType?.ToString();
        }

        public string Icon { get; private set; }

        public TabType Key { get; private set; }

        public string Title { get; private set; }

        public virtual void OnCurrentTrackChanged(Track track)
        {
        }
    }
}
