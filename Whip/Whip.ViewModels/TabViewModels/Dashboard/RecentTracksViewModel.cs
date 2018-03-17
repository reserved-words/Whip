using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using System.Linq;

namespace Whip.ViewModels.TabViewModels.Dashboard
{
    public class RecentTracksViewModel : ViewModelBase
    {
        private readonly IPlayHistoryService _service;

        public RecentTracksViewModel(IPlayHistoryService service)
        {
            _service = service;
            
            RecentTracks = new ObservableCollection<TrackPlay>();
        }

        public ObservableCollection<TrackPlay> RecentTracks { get; }

        public async void Refresh()
        {
            var trackPlays = await _service.GetRecentTrackPlays(15);
            RecentTracks.Clear();
            trackPlays.ToList().ForEach(RecentTracks.Add);
        }
    }
}
