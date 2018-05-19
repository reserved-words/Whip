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
        private readonly IScrobbleCacher _scrobbleCacher;

        private DateTime? _logsDate;
        private ICollection<Log> _logs;
        private ICollection<FailedScrobbleViewModel> _failedScrobbles;

        public SystemInfoViewModel(IWebServicesStatus webServiceStatus, IApplicationInfoService applicationInfoService, ICurrentDateTime currentDateTime,
            ILogRepository logRepository, IScrobbleCacher scrobbleCacher)
            :base(TabType.SystemInfo, IconType.InfoCircle, TabTitleSystemInfo)
        {
            _webServiceStatus = webServiceStatus;
            _applicationInfoService = applicationInfoService;
            _logRepository = logRepository;
            _currentDateTime = currentDateTime;
            _scrobbleCacher = scrobbleCacher;

            Statuses = PopulateStatuses();

            RefreshCommand = new RelayCommand(RefreshStatuses);
            DeleteScrobbleCommand = new RelayCommand<FailedScrobbleViewModel>(DeleteScrobble);
        }

        public RelayCommand RefreshCommand { get; }
        public RelayCommand<FailedScrobbleViewModel> DeleteScrobbleCommand { get; }

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

        public ICollection<FailedScrobbleViewModel> FailedScrobbles
        {
            get { return _failedScrobbles; }
            private set { Set(ref _failedScrobbles, value); }
        }

        public ICollection<Log> Logs
        {
            get { return _logs; }
            private set { Set(ref _logs, value); }
        }

        public override void OnShow(Track currentTrack)
        {
            RefreshLogs();
            RefreshStatuses();
            RefreshFailedScrobbles();
        }

        private void DeleteScrobble(FailedScrobbleViewModel scrobble)
        {
            _scrobbleCacher.Remove(scrobble.Track, scrobble.TimePlayed);
            RefreshFailedScrobbles();
        }

        private void RefreshFailedScrobbles()
        {
            FailedScrobbles = _scrobbleCacher.GetCachedScrobbles()
                .Select(s => new FailedScrobbleViewModel(s.Item1, s.Item2, s.Item3))
                .ToList();
        }

        private List<ServiceStatusViewModel> PopulateStatuses()
        {
            return new List<ServiceStatusViewModel>
            {
                new ServiceStatusViewModel(WebServiceType.Web, "Internet", IconType.Wifi, OfflineErrorMessageDetails, _webServiceStatus),
                new ServiceStatusViewModel(WebServiceType.LastFm, "Last.FM", IconType.Lastfm, LastFmOffErrorMessageDetails, _webServiceStatus),
                new ServiceStatusViewModel(WebServiceType.News, "Twitter", IconType.Twitter, null, _webServiceStatus),
                new ServiceStatusViewModel(WebServiceType.Videos, "YouTube", IconType.Youtube, null, _webServiceStatus),
                new ServiceStatusViewModel(WebServiceType.Events, "Bands In Town", IconType.Music, null, _webServiceStatus),
                new ServiceStatusViewModel(WebServiceType.Lyrics, "Lyrics", IconType.ListUl, null, _webServiceStatus)
            };
        }

        private void RefreshLogs()
        {
            if (!LogsDate.HasValue)
            {
                LogsDate = _currentDateTime.Get();
            }
            else
            {
                UpdateLogs();
            }
        }

        private void RefreshStatuses()
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
