using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Whip.ViewModels.TabViewModels
{
    public class WeeklyPlayHistoryViewModel : ViewModelBase
    {
        private readonly IPlayHistoryService _service;

        public WeeklyPlayHistoryViewModel(IPlayHistoryService service)
        {
            _service = service;
            
            TopArtists = new ObservableCollection<Statistic>();
            TopAlbums = new ObservableCollection<Statistic>();
            TopTracks = new ObservableCollection<Statistic>();
        }

        public ObservableCollection<Statistic> TopArtists { get; }
        public ObservableCollection<Statistic> TopAlbums { get; }
        public ObservableCollection<Statistic> TopTracks { get; }

        public void Refresh()
        {
            PopulateList(() => _service.GetLastWeekTopArtists(5), TopArtists);
            PopulateList(() => _service.GetLastWeekTopAlbums(5), TopAlbums);
            PopulateList(() => _service.GetLastWeekTopTracks(5), TopTracks);
        }

        private static async void PopulateList(Func<Task<ICollection<Statistic>>> function, ObservableCollection<Statistic> list)
        {
            var newList = await function();
            list.Clear();
            newList.ToList().ForEach(list.Add);
        }
    }
}
