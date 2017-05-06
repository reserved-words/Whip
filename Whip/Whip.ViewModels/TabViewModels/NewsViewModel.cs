using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whip.Common;
using Whip.Common.Model;
using Whip.Common.Model.Rss;
using Whip.Services.Interfaces;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class NewsViewModel : TabViewModelBase
    {
        private readonly IRssFeedsRepository _repository;
        private readonly IRssService _service;

        private readonly Feed _allFeeds = new Feed("All Feeds", "", "");

        private DateTime _lastUpdated = DateTime.MinValue;

        private bool _loadingPosts;
        private List<Feed> _feeds;
        private List<Post> _posts;
        private Feed _selectedFeed;
        
        public NewsViewModel(IRssFeedsRepository repository, IRssService service)
            :base(TabType.News, IconType.Rss, "News")
        {
            _repository = repository;
            _service = service;

            RefreshCommand = new RelayCommand(OnRefresh);
        }

        public RelayCommand RefreshCommand { get; private set; }

        public bool LoadingPosts
        {
            get { return _loadingPosts; }
            set { Set(ref(_loadingPosts), value); }
        }

        public List<Feed> Feeds
        {
            get { return _feeds; }
            set { Set(ref(_feeds), value); }
        }

        public List<Post> Posts
        {
            get { return _posts; }
            set { Set(ref (_posts), value); }
        }

        public Feed SelectedFeed
        {
            get { return _selectedFeed; }
            set
            {
                Set(ref(_selectedFeed), value);
                UpdatePosts(false);
            }
        }

        public override void OnShow(Track currentTrack)
        {
            UpdatePosts(false);
        }

        private void OnRefresh()
        {
            UpdatePosts(true);
        }

        private void UpdatePosts(bool force)
        {
            LoadingPosts = true;
            
            if (force || _lastUpdated.AddMinutes(ApplicationSettings.MinutesBeforeRefreshNews) <= DateTime.Now)
            {
                _lastUpdated = DateTime.Now;

                Task.Run(() => PopulatePosts()).ContinueWith(t => UpdatePosts());
            }
            else
            {
                UpdatePosts();
            }
            
        }

        private void PopulatePosts()
        {
            if (Feeds == null)
            {
                Feeds = _repository.GetFeeds();
                Feeds.Insert(0, _allFeeds);
            }

            var realFeeds = Feeds.Except(new List<Feed> { _allFeeds }).ToList();

            _service.PopulatePosts(realFeeds);
        }

        private void UpdatePosts()
        {
            if (SelectedFeed == null)
            {
                SelectedFeed = _allFeeds;
            }

            Posts = Feeds
                .SelectMany(f => f.Posts)
                .Where(p => SelectedFeed == _allFeeds || p.Feed == SelectedFeed)
                .OrderByDescending(p => p.Posted)
                .ToList();

            LoadingPosts = false;
        }
    }
}
