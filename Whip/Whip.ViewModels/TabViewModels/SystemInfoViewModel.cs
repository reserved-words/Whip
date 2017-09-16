using System;
using Whip.Common;
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

        public SystemInfoViewModel(IUserSettings userSettings, IApplicationInfoService applicationInfoService)
            :base(TabType.SystemInfo, IconType.InfoCircle, TabTitleSystemInfo)
        {
            _userSettings = userSettings;
            _applicationInfoService = applicationInfoService;
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

        public string ApplicationVersion => string.Format(ApplicationVersionFormat, _applicationInfoService.Version);
        public string ApplicationPublishDate => string.Format(ApplicationPublishedFormat, _applicationInfoService.PublishDate);
    }
}
