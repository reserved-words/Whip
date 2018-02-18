using System;
using GalaSoft.MvvmLight;
using Whip.Common.Model;

namespace Whip.ViewModels.TabViewModels.SystemInfo
{
    public class FailedScrobbleViewModel : ViewModelBase
    {
        public FailedScrobbleViewModel(Track track, DateTime timePlayed, string error)
        {
            Track = track;
            TimePlayed = timePlayed;
            Error = error;
        }

        public DateTime TimePlayed { get; }
        public Track Track { get; }
        public string Error { get; }
    }
}
