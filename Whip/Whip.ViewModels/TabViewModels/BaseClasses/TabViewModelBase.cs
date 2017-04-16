using GalaSoft.MvvmLight;
using Whip.Common;
using Whip.Common.ExtensionMethods;
using Whip.Common.Model;
using Whip.Common.Utilities;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class TabViewModelBase : ViewModelBase
    {
        private bool _isVisible;

        public TabViewModelBase(TabType type, IconType icon, string title, bool isVisible = true)
        {
            Key = type;
            Icon = icon.ToString();
            Title = title;
            IsVisible = isVisible;
        }

        public string Icon { get; private set; }

        public TabType Key { get; private set; }

        public string Title { get; private set; }

        public bool IsVisible
        {
            get { return _isVisible; }
            set { Set(ref _isVisible, value); }
        }

        public virtual void OnCurrentTrackChanged(Track track)
        {
        }
    }
}
