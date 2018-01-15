using GalaSoft.MvvmLight;
using Whip.Common;
using Resources = Whip.Common.Resources;

namespace Whip.ViewModels.TabViewModels.SystemInfo
{
    public class ServiceStatusViewModel : ViewModelBase
    {
        private bool _online;
        private string _details;
        private string _errorMessage;

        public ServiceStatusViewModel(string title, IconType iconType)
        {
            Icon = iconType.ToString();
            Title = title;
            Online = true;
        }

        public string Icon { get; }
        public string Title { get; }

        public bool Online
        {
            get { return _online; }
            set { Set(ref _online, value); }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { Set(ref _errorMessage, value); }
        }

        public string Details
        {
            get { return _details; }
            set { Set(ref _details, value); }
        }

        public string Status => Online ? Resources.Online : Resources.Offline;
    }
}
