using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Utilities;
using Whip.ViewModels.Utilities;
using static Whip.Common.Resources;

namespace Whip.ViewModels.TabViewModels
{
    public class SystemInfoViewModel : TabViewModelBase
    {
        private readonly IUserSettings _userSettings;
        private readonly IApplicationInfoService _applicationInfoService;
        private readonly ILogRepository _logRepository;
        private readonly ICurrentDateTime _currentDateTime;

        private DateTime? _logsDate;
        private ICollection<Log> _logs;

        public SystemInfoViewModel(IUserSettings userSettings, IApplicationInfoService applicationInfoService, ICurrentDateTime currentDateTime,
            ILogRepository logRepository)
            :base(TabType.SystemInfo, IconType.InfoCircle, TabTitleSystemInfo)
        {
            _userSettings = userSettings;
            _applicationInfoService = applicationInfoService;
            _logRepository = logRepository;
            _currentDateTime = currentDateTime;
        }

        public string InternetStatus => _userSettings.Offline ? Offline : Online;

        public string InternetStatusDetails => _userSettings.Offline
            ? OfflineErrorMessageDetails
            : "";

        public bool LastFmOn => _userSettings.LastFmStatus;

        public string LastFmStatus => _userSettings.LastFmStatus ? On : Off;

        public string LastFmStatusDetails => _userSettings.LastFmStatus
            ? ""
            : LastFmOffErrorMessageDetails;

        public string LastFmErrorMessage => _userSettings.LastFmErrorMessage;

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

        private void UpdateLogs()
        {
            Logs = !LogsDate.HasValue
                ? null
                : _logRepository.GetLogs(LogsDate.Value).OrderByDescending(l => l.Id).ToList();
        }
    }
}
