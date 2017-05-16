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

        public SearchViewModel(Common.Singletons.Library library, IMessenger messenger, ITrackSearchService trackSearchService,
            IPlaylistRepository repository)
            :base(TabType.Search, IconType.Search, "Library Search")
        {
            _library = library;
            _messenger = messenger;
            _repository = repository;
            _trackSearchService = trackSearchService;

            Criteria = new ObservableCollection<CriteriaGroupViewModel>();

            AddNewCriteriaGroupCommand = new RelayCommand(OnAddNewCriteriaGroup);
            SearchCommand = new RelayCommand(OnSearch);
            ClearCommand = new RelayCommand(OnClear);
            SaveAsPlaylistCommand = new RelayCommand(OnSaveAsPlaylist);
            PlayCommand = new RelayCommand(OnPlay);
            EditCommand = new RelayCommand(OnEdit);
            RemoveGroupCommand = new RelayCommand<CriteriaGroupViewModel>(OnRemoveGroup);
        }

        public RelayCommand AddNewCriteriaGroupCommand { get; private set; }
        public RelayCommand SearchCommand { get; private set; }
        public RelayCommand ClearCommand { get; private set; }
        public RelayCommand SaveAsPlaylistCommand { get; private set; }
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
            set { Set(ref (_selectedTrack), value); }
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
            OnClear();
        }

        private void OnEdit()
        {
            _messenger.Send(new EditTrackMessage(SelectedTrack));
        }

        private void OnPlay()
        {
            _messenger.Send(new PlayPlaylistMessage("Search Results", SortType.Random, Results, SelectedTrack));
        }

        private void OnSaveAsPlaylist()
        {
            // Could add an option to save as either criteria OR ordered playlist
            // Ordered playlist would preserve same tracks even if library changed
            // Criteria playlist would update with ilbrary

            var getPlaylistNameViewModel = new EnterStringViewModel(_messenger, "Save Playlist", "Enter playlist name");

            _messenger.Send(new ShowDialogMessage(getPlaylistNameViewModel));

            var playlistTitle = getPlaylistNameViewModel.Result;

            var playlist = new CriteriaPlaylist(0, playlistTitle)
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

            _repository.Save(playlist);
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
