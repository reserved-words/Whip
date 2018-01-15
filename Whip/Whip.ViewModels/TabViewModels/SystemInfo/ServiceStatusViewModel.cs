using System;
using GalaSoft.MvvmLight;
using Whip.Common;
using Resources = Whip.Common.Resources;
using Whip.Services.Interfaces.Singletons;

namespace Whip.ViewModels.TabViewModels.SystemInfo
{
    public class ServiceStatusViewModel : ViewModelBase
    {
        private readonly IWebServicesStatus _webStatusService;
        private readonly Func<IWebServicesStatus, bool> _funcOnline;
        private readonly Func<IWebServicesStatus, string> _funcErrorMessage;

        public ServiceStatusViewModel(string title, IconType iconType, string errorDetails, IWebServicesStatus webStatusService, 
            Func<IWebServicesStatus, bool> funcOnline, Func<IWebServicesStatus, string> funcErrorMessage)
        {
            Icon = iconType.ToString();
            Title = title;
            Details = errorDetails;

            _funcOnline = funcOnline;
            _funcErrorMessage = funcErrorMessage;
            _webStatusService = webStatusService;
        }

        public string Icon { get; }
        public string Title { get; }

        public bool Online => _funcOnline(_webStatusService);

        public string ErrorMessage => _funcErrorMessage(_webStatusService);

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
