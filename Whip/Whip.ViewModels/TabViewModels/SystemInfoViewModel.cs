using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using Whip.Common;
using Whip.Common.Enums;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.Services.Interfaces.Utilities;
using Whip.ViewModels.TabViewModels.SystemInfo;
using Whip.ViewModels.Utilities;
using static Whip.Common.Resources;

namespace Whip.ViewModels.TabViewModels
{
    public class SystemInfoViewModel : TabViewModelBase
    {
        private readonly IWebServicesStatus _webServiceStatus;
        private readonly IApplicationInfoService _applicationInfoService;
        private readonly ILogRepository _logRepository;
        private readonly ICurrentDateTime _currentDateTime;

        private DateTime? _logsDate;
        private ICollection<Log> _logs;

        public SystemInfoViewModel(IWebServicesStatus webServiceStatus, IApplicationInfoService applicationInfoService, ICurrentDateTime currentDateTime,
            ILogRepository logRepository)
            :base(TabType.SystemInfo, IconType.InfoCircle, TabTitleSystemInfo)
        {
            _webServiceStatus = webServiceStatus;
            _applicationInfoService = applicationInfoService;
            _logRepository = logRepository;
            _currentDateTime = currentDateTime;

            Statuses = PopulateStatuses();

            RefreshCommand = new RelayCommand(Refresh);
        }

        public RelayCommand RefreshCommand { get; }

        public List<ServiceStatusViewModel> Statuses { get; }

        public string ApplicationVersion => _applicationInfoService.Version;
        public string ApplicationPublishDate => _applicationInfoService.PublishDate.ToString(ApplicationPublishedFormat);

        public DateTime? LogsDate
        {
            get { return _logsDate; }
            set
            {
                Set(ref _logsDate, value);
                UpdateLogs();
            }
        }

        public ICollection<Log> Logs
        {
            get { return _logs; }
            private set { Set(ref _logs, value); }
        }

        public override void OnShow(Track currentTrack)
        {
            if (!LogsDate.HasValue)
            {
                LogsDate = _currentDateTime.Get();
            }

            Refresh();
        }

        private List<ServiceStatusViewModel> PopulateStatuses()
        {
            return new List<ServiceStatusViewModel>
            {
                new ServiceStatusViewModel(WebServiceType.Web, "Internet", IconType.Wifi, OfflineErrorMessageDetails, _webServiceStatus),
                new ServiceStatusViewModel(WebServiceType.LastFm, "Last.FM", IconType.Lastfm, LastFmOffErrorMessageDetails, _webServiceStatus),
                new ServiceStatusViewModel(WebServiceType.News, "Twitter", IconType.Twitter, TwitterOffErrorMessageDetails, _webServiceStatus),
                new ServiceStatusViewModel(WebServiceType.Videos, "YouTube", IconType.Youtube, YouTubeOffErrorMessageDetails, _webServiceStatus),
                new ServiceStatusViewModel(WebServiceType.Events, "Bands In Town", IconType.Music, BandsInTownOffErrorMessageDetails, _webServiceStatus)
            };
        }

        private void Refresh()
        {
            Statuses.ForEach(s => s.Refresh());
        }

        private void UpdateLogs()
        {
            Logs = !LogsDate.HasValue
                ? null
                : _logRepository.GetLogs(LogsDate.Value).OrderByDescending(l => l.Id).ToList();
        }
    }
}
