using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.Services.Interfaces.Utilities;
using Whip.ViewModels.Utilities;
using static Whip.Common.Resources;

namespace Whip.ViewModels.TabViewModels
{
    public class SystemInfoViewModel : TabViewModelBase
    {
        private readonly IWebServicesStatus _userSettings;
        private readonly IApplicationInfoService _applicationInfoService;
        private readonly ILogRepository _logRepository;
        private readonly ICurrentDateTime _currentDateTime;

        private DateTime? _logsDate;
        private ICollection<Log> _logs;

        public SystemInfoViewModel(IWebServicesStatus userSettings, IApplicationInfoService applicationInfoService, ICurrentDateTime currentDateTime,
            ILogRepository logRepository)
            :base(TabType.SystemInfo, IconType.InfoCircle, TabTitleSystemInfo)
        {
            _userSettings = userSettings;
            _applicationInfoService = applicationInfoService;
            _logRepository = logRepository;
            _currentDateTime = currentDateTime;

            RefreshCommand = new RelayCommand(Refresh);
        }

        public RelayCommand RefreshCommand { get; }

        public string InternetStatus => _userSettings.Offline ? Offline : Online;

        public string InternetStatusDetails => _userSettings.Offline
            ? OfflineErrorMessageDetails
            : "";

        public bool LastFmOn => _userSettings.LastFmStatus;

        public string LastFmErrorMessage => _userSettings.LastFmErrorMessage;

        public string LastFmStatus => _userSettings.LastFmStatus ? Online : Offline;

        public string LastFmStatusDetails => _userSettings.LastFmStatus
            ? ""
            : LastFmOffErrorMessageDetails;

        // TO DO
        public bool BandsInTownOn => true;
        public string BandsInTownErrorMessage => "";
        public string BandsInTownStatus => Online;
        public string BandsInTownStatusDetails => "";
        public bool TwitterOn => true;
        public string TwitterErrorMessage => "";
        public string TwitterStatus => Online;
        public string TwitterStatusDetails => "";
        public bool YouTubeOn => true;
        public string YouTubeErrorMessage => "";
        public string YouTubeStatus => Online;
        public string YouTubeStatusDetails => "";

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
        }

        private void Refresh()
        {
            RaisePropertyChanged(nameof(InternetStatus));
            RaisePropertyChanged(nameof(InternetStatusDetails));
            RaisePropertyChanged(nameof(LastFmOn));
            RaisePropertyChanged(nameof(LastFmStatus));
            RaisePropertyChanged(nameof(LastFmStatusDetails));
        }

        private void UpdateLogs()
        {
            Logs = !LogsDate.HasValue
                ? null
                : _logRepository.GetLogs(LogsDate.Value).OrderByDescending(l => l.Id).ToList();
        }
    }
}
