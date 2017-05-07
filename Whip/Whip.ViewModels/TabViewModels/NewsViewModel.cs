using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Whip.Common;
using Whip.Common.Model;
using Whip.Common.Model.Rss;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;
using Whip.ViewModels.Utilities;
using Whip.ViewModels.Windows;

namespace Whip.ViewModels.TabViewModels
{
    public class NewsViewModel : TabViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly IRssFeedsRepository _repository;
        private readonly IRssService _service;

        private readonly Feed _allFeeds = new Feed("All Feeds", "", "", "", "");

        private DateTime _lastUpdated = DateTime.MinValue;

        private bool _loadingPosts;
        private List<Post> _posts;
        private List<Feed> _realFeeds;
        private Feed _selectedFeed;
        
        public NewsViewModel(IMessenger messenger, IRssFeedsRepository repository, IRssService service)
            :base(TabType.News, IconType.Rss, "News")
        {
            _messenger = messenger;
            _repository = repository;
            _service = service;

            AddFeedCommand = new RelayCommand(OnAddFeed);
            RefreshCommand = new RelayCommand(OnRefresh);

            DeleteFeedCommand = new RelayCommand(OnDeleteFeed, IsFeedSelected);
            EditFeedCommand = new RelayCommand(OnEditFeed, IsFeedSelected);

            Feeds = new ObservableCollection<Feed> { _allFeeds };
        }

        private bool IsFeedSelected()
        {
            return SelectedFeed != null && SelectedFeed != _allFeeds;
        }

        public RelayCommand AddFeedCommand { get; private set; }
        public RelayCommand DeleteFeedCommand { get; private set; }
        public RelayCommand EditFeedCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }
        
        public bool LoadingPosts
        {
            get { return _loadingPosts; }
            set { Set(ref(_loadingPosts), value); }
        }

        public ObservableCollection<Feed> Feeds { get; private set; }

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
                EditFeedCommand.RaiseCanExecuteChanged();
                DeleteFeedCommand.RaiseCanExecuteChanged();
                FilterPosts();
            }
        }
        
        public override void OnShow(Track currentTrack)
        {
            if (Feeds.Count == 1)
            {
                UpdateFeeds(true);
            }
            else
            {
                SelectedFeed = _allFeeds;
            }
        }

        private void OnAddFeed()
        {
            var editFeedViewModel = new EditFeedViewModel(_messenger);
            _messenger.Send(new ShowDialogMessage(editFeedViewModel));

            if (editFeedViewModel.Saved)
            {
                _realFeeds.Add(editFeedViewModel.Feed);
                SaveFeeds();
                PopulatePosts(editFeedViewModel.Feed);
            }
        }

        private void OnDeleteFeed()
        {
            var confirmationViewModel = new ConfirmationViewModel(_messenger, "Delete Feed", "Are you sure you want to delete this feed?", ConfirmationViewModel.ConfirmationType.YesNo);
            _messenger.Send(new ShowDialogMessage(confirmationViewModel));

            if (confirmationViewModel.Result)
            {
                _realFeeds.Remove(SelectedFeed);
                SelectedFeed = _allFeeds;
                SaveFeeds();
            }
        }

        private void OnEditFeed()
        {
            var editFeedViewModel = new EditFeedViewModel(_messenger, SelectedFeed);
            _messenger.Send(new ShowDialogMessage(editFeedViewModel));

            if (editFeedViewModel.Saved)
            {
                SaveFeeds();
            }
        }

        private void OnRefresh()
        {
            UpdatePosts(true);
        }

        private void SaveFeeds()
        {
            _repository.SaveFeeds(_realFeeds);
            UpdateFeeds(false);
        }

        private void UpdatePosts(bool force)
        {
            if (force || _lastUpdated.AddMinutes(ApplicationSettings.MinutesBeforeRefreshNews) <= DateTime.Now)
            {
                _lastUpdated = DateTime.Now;

                PopulatePosts();
            }
            else
            {
                FilterPosts();
            }
        }

        private void PopulatePosts(Feed feed = null)
        {
            LoadingPosts = true;

            _service.PopulatePosts(feed == null ? _realFeeds : new List<Feed> { feed })
                .ContinueWith(t => FilterPosts(feed), TaskScheduler.FromCurrentSynchronizationContext())
                .ContinueWith(t => { LoadingPosts = false; }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void UpdateFeeds(bool fromRepository)
        {
            var selectedFeed = SelectedFeed;

            Feeds.Where(f => f != _allFeeds).ToList().ForEach(f => Feeds.Remove(f));

            if (fromRepository)
            {
                _realFeeds = _repository.GetFeeds().OrderBy(f => f.Title).ToList();
                UpdatePosts(true);
            }
            
            _realFeeds.ForEach(f => Feeds.Add(f));

            if (Feeds.Contains(selectedFeed))
            {
                SelectedFeed = selectedFeed;
            }
            else
            {
                SelectedFeed = _allFeeds;
            }
        }

        private void FilterPosts(Feed feed = null)
        {
            if (feed != null)
            {
                SelectedFeed = feed;
                return;
            }

            Posts = _realFeeds
                .SelectMany(f => f.Posts)
                .Where(p => SelectedFeed == _allFeeds || p.Feed == SelectedFeed)
                .OrderByDescending(p => p.Posted)
                .ToList();
        }
    }
}
