using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
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
        private List<Feed> _feeds;
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
                EditFeedCommand.RaiseCanExecuteChanged();
                DeleteFeedCommand.RaiseCanExecuteChanged();
                UpdatePosts(false, false);
            }
        }
        
        public override void OnShow(Track currentTrack)
        {
            UpdatePosts(false, false);
        }

        private void OnAddFeed()
        {
            var editFeedViewModel = new EditFeedViewModel(_messenger);
            _messenger.Send(new ShowDialogMessage(editFeedViewModel));
            if (editFeedViewModel.Saved)
            {
                _realFeeds.Add(editFeedViewModel.Feed);
                SaveFeeds();
            }
        }

        private void OnDeleteFeed()
        {
            var confirmationViewModel = new ConfirmationViewModel(_messenger, "Delete Feed", "Are you sure you want to delete this feed?", ConfirmationViewModel.ConfirmationType.YesNo);
            _messenger.Send(new ShowDialogMessage(confirmationViewModel));
            if (confirmationViewModel.Result)
            {
                _realFeeds.Remove(SelectedFeed);
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
            UpdatePosts(true, false);
        }

        private void SaveFeeds()
        {
            _repository.SaveFeeds(_realFeeds);
            UpdatePosts(true, true);
        }

        private void UpdatePosts(bool force, bool updateFeeds)
        {
            LoadingPosts = true;
            
            if (force || updateFeeds || _lastUpdated.AddMinutes(ApplicationSettings.MinutesBeforeRefreshNews) <= DateTime.Now)
            {
                _lastUpdated = DateTime.Now;

                Task.Run(() => PopulatePosts(updateFeeds)).ContinueWith(t => UpdatePosts(), TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                UpdatePosts();
            }
            
        }

        private void PopulatePosts(bool updateFeeds)
        {
            if (updateFeeds || Feeds == null)
            {
                _realFeeds = _repository.GetFeeds().OrderBy(f => f.Title).ToList();
                Feeds = new List<Feed>(_realFeeds);
                Feeds.Insert(0, _allFeeds);
            }

            _service.PopulatePosts(_realFeeds);
        }

        private void UpdatePosts()
        {
            if (SelectedFeed == null)
            {
                SelectedFeed = _allFeeds;
            }

            Posts = _realFeeds
                .SelectMany(f => f.Posts)
                .Where(p => SelectedFeed == _allFeeds || p.Feed == SelectedFeed)
                .OrderByDescending(p => p.Posted)
                .ToList();

            LoadingPosts = false;
        }
    }
}
