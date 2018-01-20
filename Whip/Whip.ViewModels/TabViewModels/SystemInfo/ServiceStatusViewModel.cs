using System;
using GalaSoft.MvvmLight;
using Whip.Common;
using Whip.Common.Enums;
using Resources = Whip.Common.Resources;
using Whip.Services.Interfaces.Singletons;

namespace Whip.ViewModels.TabViewModels.SystemInfo
{
    public class ServiceStatusViewModel : ViewModelBase
    {
        private readonly string _details;
        private readonly WebServiceType _type;
        private readonly IWebServicesStatus _webStatusService;

        public ServiceStatusViewModel(WebServiceType type, string title, IconType iconType, string errorDetails, IWebServicesStatus webStatusService)
        {
            Icon = iconType.ToString();
            Title = title;
            _details = errorDetails;

            _type = type;
            _webStatusService = webStatusService;
        }

        public string Icon { get; }
        public string Title { get; }

        public bool Online => _webStatusService.IsOnline(_type);

        public string ErrorMessage => string.IsNullOrEmpty(_details) 
            ? _webStatusService.GetErrorMessage(_type)
            : _details + Environment.NewLine + Environment.NewLine + _webStatusService.GetErrorMessage(_type);

        public string Status => Online ? Resources.Online : Resources.Offline;

        public DateTime TimeUpdated => _webStatusService.GetTimeUpdated(_type);

        public void Refresh()
        {
            RaisePropertyChanged(nameof(Online));
            RaisePropertyChanged(nameof(Status));
            RaisePropertyChanged(nameof(ErrorMessage));
        }
    }
}
