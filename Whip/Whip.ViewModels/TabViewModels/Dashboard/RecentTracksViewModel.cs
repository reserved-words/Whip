using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using System.Linq;

namespace Whip.ViewModels.TabViewModels.Dashboard
{
    public class RecentTracksViewModel : ViewModelBase
    {
        private readonly IRecentTracksService _service;
        private readonly IUserSettings _userSettings;

        public RecentTracksViewModel(IRecentTracksService service, IUserSettings userSettings)
        {
            _service = service;
            _userSettings = userSettings;

            RecentTracks = new ObservableCollection<TrackPlay>();
        }

        public ObservableCollection<TrackPlay> RecentTracks { get; }

        public async void Refresh()
        {
            RecentTracks.Clear();

            var trackPlays = await _service.GetRecentTrackPlays(_userSettings.LastFmUsername, 20);

            trackPlays.ToList().ForEach(RecentTracks.Add);
        }
    }
}
