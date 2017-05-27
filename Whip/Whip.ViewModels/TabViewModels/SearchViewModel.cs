using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using Whip.Common;
using Whip.Common.Model;
using Whip.ViewModels.TabViewModels.Playlists;
using Whip.ViewModels.Utilities;
using System;
using System.Linq;
using System.Collections.Generic;
using Whip.Services.Interfaces;
using Whip.Common.Interfaces;
using Whip.Common.Model.Playlists.Criteria;
using GalaSoft.MvvmLight.Messaging;
using Whip.ViewModels.Messages;
using Whip.ViewModels.Windows;

namespace Whip.ViewModels.TabViewModels
{
    public class SearchViewModel : TabViewModelBase, ITrackCriteria
    {
        private readonly Common.Singletons.Library _library;
        private readonly IMessenger _messenger;
        private readonly ITrackSearchService _trackSearchService;
        private readonly IPlaylistRepository _repository;

        private List<Track> _results;
        private Track _selectedTrack;

        private PropertyName? _orderByProperty;
        private bool _orderByDescending;
        private int? _maxTracks;

        private Lazy<List<string>> _tags;
        private Lazy<List<string>> _groupings;
        private Lazy<List<string>> _countries;
        private Lazy<List<string>> _states;
        private Lazy<List<string>> _cities;

        public SearchViewModel(Common.Singletons.Library library, IMessenger messenger, ITrackSearchService trackSearchService,
            IPlaylistRepository repository, TrackContextMenuViewModel trackContextMenu)
            :base(TabType.Search, IconType.Search, "Library Search")
        {
            _library = library;
            _messenger = messenger;
            _repository = repository;
            _trackSearchService = trackSearchService;

            TrackContextMenu = trackContextMenu;

            Criteria = new ObservableCollection<CriteriaGroupViewModel>();

            AddNewCriteriaGroupCommand = new RelayCommand(OnAddNewCriteriaGroup);
            SearchCommand = new RelayCommand(OnSearch);
            ClearCommand = new RelayCommand(OnClear);
            SaveAsCriteriaPlaylistCommand = new RelayCommand(OnSaveAsCriteriaPlaylist);
            SaveAsOrderedPlaylistCommand = new RelayCommand(OnSaveAsOrderedPlaylist);
            PlayCommand = new RelayCommand(OnPlay);
            EditCommand = new RelayCommand(OnEdit);
            RemoveGroupCommand = new RelayCommand<CriteriaGroupViewModel>(OnRemoveGroup);
        }

        public TrackContextMenuViewModel TrackContextMenu { get; private set; }

        public RelayCommand AddNewCriteriaGroupCommand { get; private set; }
        public RelayCommand SearchCommand { get; private set; }
        public RelayCommand ClearCommand { get; private set; }
        public RelayCommand SaveAsCriteriaPlaylistCommand { get; private set; }
        public RelayCommand SaveAsOrderedPlaylistCommand { get; private set; }
        public RelayCommand PlayCommand { get; private set; }
        public RelayCommand EditCommand { get; private set; }
        public RelayCommand<CriteriaGroupViewModel> RemoveGroupCommand { get; private set; }

        public ObservableCollection<CriteriaGroupViewModel> Criteria { get; private set; }

        public List<Track> Results
        {
            get { return _results; }
            private set { Set(ref (_results), value); }
        }

        public Track SelectedTrack
        {
            get { return _selectedTrack; }
            set
            {
                Set(ref (_selectedTrack), value);
                TrackContextMenu.SetTrack(_selectedTrack);
            }
        }

        public List<CriteriaGroup> CriteriaGroups
        {
            get
            {
                var criteriaGroups = new List<CriteriaGroup>();

                foreach (var group in Criteria.Where(g => g.Criteria.Any()))
                {
                    var criteriaGroup = new CriteriaGroup();

                    foreach (var criteria in group.Criteria.Where(g => g.PropertyName != null))
                    {
                        switch (criteria.PropertyOwner)
                        {
                            case PropertyOwner.Track:
                                criteriaGroup.TrackCriteria.Add(_trackSearchService.GetTrackCriteria(criteria.PropertyName.Value, criteria.CriteriaType.Value, criteria.ValueString));
                                break;
                            case PropertyOwner.Disc:
                                criteriaGroup.DiscCriteria.Add(_trackSearchService.GetDiscCriteria(criteria.PropertyName.Value, criteria.CriteriaType.Value, criteria.ValueString));
                                break;
                            case PropertyOwner.Album:
                                criteriaGroup.AlbumCriteria.Add(_trackSearchService.GetAlbumCriteria(criteria.PropertyName.Value, criteria.CriteriaType.Value, criteria.ValueString));
                                break;
                            case PropertyOwner.Artist:
                                criteriaGroup.ArtistCriteria.Add(_trackSearchService.GetArtistCriteria(criteria.PropertyName.Value, criteria.CriteriaType.Value, criteria.ValueString));
                                break;
                            default:
                                throw new InvalidOperationException("Invalid property owner");
                        }
                    }

                    criteriaGroups.Add(criteriaGroup);
                }

                return criteriaGroups;
            }
        }

        public PropertyName? OrderByProperty
        {
            get { return _orderByProperty; }
            set { Set(ref _orderByProperty, value); }
        }

        public bool OrderByDescending
        {
            get { return _orderByDescending; }
            set { Set(ref _orderByDescending, value); }
        }

        public int? MaxTracks
        {
            get { return _maxTracks; }
            set { Set(ref _maxTracks, value); }
        }

        public override void OnShow(Track currentTrack)
        {
            _tags = new Lazy<List<string>>(() => _library.Artists.SelectMany(a => a.Tracks).SelectMany(t => t.Tags).Distinct().OrderBy(g => g).ToList());
            _groupings = new Lazy<List<string>>(() => _library.Artists.Select(a => a.Grouping).Distinct().OrderBy(g => g).ToList());
            _countries = new Lazy<List<string>>(() => _library.Artists.Select(a => a.City.Country).Distinct().OrderBy(g => g).ToList());
            _states = new Lazy<List<string>>(() => _library.Artists.Select(a => a.City.State).Distinct().OrderBy(g => g).ToList());
            _cities = new Lazy<List<string>>(() => _library.Artists.Select(a => a.City.Name).Distinct().OrderBy(g => g).ToList());

            if (Criteria.Any())
                return;

            OnClear();
        }

        public List<string> Tags => _tags.Value;
        public List<string> Cities => _cities.Value;
        public List<string> States => _states.Value;
        public List<string> Countries => _countries.Value;
        public List<string> Groupings => _groupings.Value;

        private void OnEdit()
        {
            _messenger.Send(new EditTrackMessage(SelectedTrack));
        }

        private void OnPlay()
        {
            _messenger.Send(new PlayPlaylistMessage("Search Results", SortType.Random, Results, SelectedTrack));
        }

        private void OnSaveAsCriteriaPlaylist()
        {
            var getPlaylistNameViewModel = new EnterStringViewModel(_messenger, "Save Playlist", "Enter playlist name");

            _messenger.Send(new ShowDialogMessage(getPlaylistNameViewModel));

            _repository.Save(GetCriteriaPlaylist(getPlaylistNameViewModel.Result));
        }

        private void OnSaveAsOrderedPlaylist()
        {
            var getPlaylistNameViewModel = new EnterStringViewModel(_messenger, "Save Playlist", "Enter playlist name");

            _messenger.Send(new ShowDialogMessage(getPlaylistNameViewModel));

            _repository.Save(GetOrderedPlaylist(getPlaylistNameViewModel.Result));
        }

        private CriteriaPlaylist GetCriteriaPlaylist(string title)
        {
            var playlist = new CriteriaPlaylist(0, title)
            {
                OrderByProperty = OrderByProperty,
                OrderByDescending = OrderByDescending,
                MaxTracks = MaxTracks
            };

            foreach (var group in Criteria.Where(g => g.Criteria.Any()))
            {
                var criteriaGroup = new CriteriaGroup();

                foreach (var criteria in group.Criteria.Where(g => g.PropertyName != null))
                {
                    switch (criteria.PropertyOwner)
                    {
                        case PropertyOwner.Track:
                            criteriaGroup.TrackCriteria.Add(_trackSearchService.GetTrackCriteria(criteria.PropertyName.Value, criteria.CriteriaType.Value, criteria.ValueString));
                            break;
                        case PropertyOwner.Disc:
                            criteriaGroup.DiscCriteria.Add(_trackSearchService.GetDiscCriteria(criteria.PropertyName.Value, criteria.CriteriaType.Value, criteria.ValueString));
                            break;
                        case PropertyOwner.Album:
                            criteriaGroup.AlbumCriteria.Add(_trackSearchService.GetAlbumCriteria(criteria.PropertyName.Value, criteria.CriteriaType.Value, criteria.ValueString));
                            break;
                        case PropertyOwner.Artist:
                            criteriaGroup.ArtistCriteria.Add(_trackSearchService.GetArtistCriteria(criteria.PropertyName.Value, criteria.CriteriaType.Value, criteria.ValueString));
                            break;
                        default:
                            throw new InvalidOperationException("Invalid property owner");
                    }
                }

                playlist.CriteriaGroups.Add(criteriaGroup);
            }

            return playlist;
        }

        private OrderedPlaylist GetOrderedPlaylist(string title)
        {
            return new OrderedPlaylist(0, title)
            {
                Tracks = Results.Select(t => t.File.FullPath).ToList()
            };
        }

        private void OnClear()
        {
            Criteria.Clear();

            OrderByProperty = null;
            OrderByDescending = false;
            MaxTracks = null;

            Criteria.Add(new CriteriaGroupViewModel(new List<CriteriaViewModel> { new CriteriaViewModel() })
            {
                IsLastGroup = true
            });

            Results = new List<Track>();
        }

        private void OnRemoveGroup(CriteriaGroupViewModel group)
        {
            Criteria.Remove(group);

            if (Criteria.Any())
            {
                Criteria.Last().IsLastGroup = true;
            }
        }

        private void OnSearch()
        {
            Results = _trackSearchService.GetTracks(this);

            if (!Results.Any())
            {
                _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Info, "Library Search", "The search criteria did not return any results"));
            }
        }

        private void OnAddNewCriteriaGroup()
        {
            if (Criteria.Any())
            {
                Criteria.Last().IsLastGroup = false;
            }

            Criteria.Add(new CriteriaGroupViewModel
            {
                IsLastGroup = true
            });
        }
    }
}
