using GalaSoft.MvvmLight;
using Whip.Common;
using Whip.Common.Enums;
using Resources = Whip.Common.Resources;
using Whip.Services.Interfaces.Singletons;

namespace Whip.ViewModels.TabViewModels.SystemInfo
{
    public class ServiceStatusViewModel : ViewModelBase
    {
        private readonly WebServiceType _type;
        private readonly IWebServicesStatus _webStatusService;
        
        public ServiceStatusViewModel(WebServiceType type, string title, IconType iconType, string errorDetails, IWebServicesStatus webStatusService)
        {
            Icon = iconType.ToString();
            Title = title;
            Details = errorDetails;

            _type = type;
            _webStatusService = webStatusService;
        }

        public string Icon { get; }
        public string Title { get; }

        public bool Online => _webStatusService.IsOnline(_type);

        public string ErrorMessage => _webStatusService.GetErrorMessage(_type);

        public string Details { get; }

        public string Status => Online ? Resources.Online : Resources.Offline;

        public void Refresh()
        {
            RaisePropertyChanged(nameof(Online));
            RaisePropertyChanged(nameof(Status));
            RaisePropertyChanged(nameof(ErrorMessage));
        }
    }
}
