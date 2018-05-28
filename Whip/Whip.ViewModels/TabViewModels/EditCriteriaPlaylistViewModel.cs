using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Linq;
using Whip.Common;
using Whip.Common.Model;
using Whip.Common.Model.Playlists.Criteria;
using Whip.ViewModels.Utilities;
using Whip.ViewModels.TabViewModels.Playlists;
using Whip.Services.Interfaces;
using System;
using System.Collections.Generic;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.TabViewModels
{
    public class EditCriteriaPlaylistViewModel : EditableTabViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly ITrackSearchService _trackSearchService;
        private readonly ITrackCriteriaService _trackCriteriaService;
        private readonly IPlaylistsService _repository;
        private readonly Common.Singletons.Library _library;

        private CriteriaPlaylist _playlist;
        
        private List<Track> _tracks;
        private string _playlistTitle;
        private PropertyName? _orderByProperty;
        private bool _orderByDescending;
        private int? _maxTracks;
        private Track _selectedTrack;
        
        private Lazy<List<string>> _tags;
        private Lazy<List<string>> _groupings;
        private Lazy<List<string>> _countries;
        private Lazy<List<string>> _states;
        private Lazy<List<string>> _cities;

        public EditCriteriaPlaylistViewModel(IMessenger messenger, IPlaylistsService repository, ITrackSearchService trackSearchService,
            Common.Singletons.Library library, ITrackCriteriaService trackCriteriaService)
            :base(TabType.Playlists, IconType.Cog, "Edit Playlist", messenger, false)
        {
            _library = library;
            _messenger = messenger;
            _trackCriteriaService = trackCriteriaService;
            _trackSearchService = trackSearchService;
            _repository = repository;

            AddNewCriteriaGroupCommand = new RelayCommand(OnAddNewCriteriaGroup);
            PreviewResultsCommand = new RelayCommand(OnPreviewResults);
            RemoveGroupCommand = new RelayCommand<CriteriaGroupViewModel>(OnRemoveGroup);
        }

        public RelayCommand AddNewCriteriaGroupCommand { get; private set; }
        public RelayCommand PreviewResultsCommand { get; private set; }
        public RelayCommand<CriteriaGroupViewModel> RemoveGroupCommand { get; private set; }

        public string PlaylistTitle
        {
            get { return _playlistTitle; }
            set { SetModified(nameof(PlaylistTitle), ref _playlistTitle, value); }
        }

        public PropertyName? OrderByProperty
        {
            get { return _orderByProperty; }
            set { SetModified(nameof(OrderByProperty), ref _orderByProperty, value); }
        }

        public bool OrderByDescending
        {
            get { return _orderByDescending; }
            set { SetModified(nameof(OrderByDescending), ref _orderByDescending, value); }
        }

        public int? MaxTracks
        {
            get { return _maxTracks; }
            set { SetModified(nameof(MaxTracks), ref _maxTracks, value); }
        }

        public List<Track> Tracks
        {
            get { return _tracks; }
            set { Set(ref _tracks, value); }
        }

        public Track SelectedTrack
        {
            get { return _selectedTrack; }
            set
            {
                Set(ref _selectedTrack, value);
            }
        }

        public ObservableCollection<CriteriaGroupViewModel> Criteria { get; private set; }

        protected override string ErrorMessage
        {
            get
            {
                var errorMessage = "";

                if (string.IsNullOrEmpty(PlaylistTitle))
                {
                    errorMessage = errorMessage + "You must select a playlist title" + Environment.NewLine;
                }
                else if (!_repository.ValidatePlaylistTitle(PlaylistTitle, _playlist.Id))
                {
                    errorMessage = errorMessage + string.Format("There is already a playlist called {0}{1}", PlaylistTitle, Environment.NewLine);
                }

                if (!Criteria.Any(c => c.Criteria.Any(cr => cr.PropertyName != null)) && (!MaxTracks.HasValue || OrderByProperty == null))
                {
                    errorMessage = errorMessage 
                        + "You must select at least one filter criteria or set a sort property and a maximum number of tracks" 
                        + Environment.NewLine;
                }
                else if (Criteria.Any(c => c.Criteria.Any(cr => 
                    cr.PropertyName != null
                        && !(cr.CriteriaType == CriteriaType.IsTrue || cr.CriteriaType == CriteriaType.IsFalse)
                        && (cr.CriteriaType == null || string.IsNullOrEmpty(cr.ValueString)))))
                {
                    errorMessage = errorMessage + "You must select a criteria type and value for each criterion";
                }

                return errorMessage;
            }
        }

        protected override bool CustomCancel()
        {
            return true;
        }

        protected override bool CustomSave()
        {
            _playlist = CreatePlaylist(_playlist);

            _repository.Save(_playlist);

            return true;
        }

        public void Edit(CriteriaPlaylist playlist)
        {
            _playlist = playlist;

            _tags = new Lazy<List<string>>(() => _library.Artists.SelectMany(a => a.Tracks).SelectMany(t => t.Tags).Distinct().OrderBy(g => g).ToList());
            _groupings = new Lazy<List<string>>(() => _library.Artists.Select(a => a.Grouping).Distinct().OrderBy(g => g).ToList());
            _countries = new Lazy<List<string>>(() => _library.Artists.Select(a => a.City.Country).Distinct().OrderBy(g => g).ToList());
            _states = new Lazy<List<string>>(() => _library.Artists.Select(a => a.City.State).Distinct().OrderBy(g => g).ToList());
            _cities = new Lazy<List<string>>(() => _library.Artists.Select(a => a.City.Name).Distinct().OrderBy(g => g).ToList());

            Set(nameof(PlaylistTitle), ref _playlistTitle, _playlist.Title);
            Set(nameof(OrderByProperty), ref _orderByProperty, _playlist.OrderByProperty);
            Set(nameof(OrderByDescending), ref _orderByDescending, _playlist.OrderByDescending);
            Set(nameof(MaxTracks), ref _maxTracks, _playlist.MaxTracks);
            
            Criteria = new ObservableCollection<CriteriaGroupViewModel>();

            foreach (var cg in playlist.CriteriaGroups)
            {
                Criteria.Add(new CriteriaGroupViewModel(cg.TrackCriteria.OfType<Criteria>()
                    .Concat(cg.DiscCriteria)
                    .Concat(cg.AlbumCriteria)
                    .Concat(cg.ArtistCriteria)
                    .Select(c => new CriteriaViewModel(c))
                    .ToList()));
            }

            OnPreviewResults(false);

            if (!Criteria.Any())
            {
                Criteria.Add(new CriteriaGroupViewModel(new List<CriteriaViewModel> { new CriteriaViewModel() }));
            }

            Criteria.Last().IsLastGroup = true;
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

        private void OnPreviewResults()
        {
            OnPreviewResults(true);
        }

        private void OnPreviewResults(bool showMessageIfNoResults)
        {
            Tracks = _trackSearchService.GetTracks(CreatePlaylist(_playlist));

            if (showMessageIfNoResults && !Tracks.Any())
            {
                _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Info, "Criteria Playlist", "No tracks meet the selected criteria"));
            }
        }

        private void OnRemoveGroup(CriteriaGroupViewModel group)
        {
            Criteria.Remove(group);

            if (Criteria.Any())
            {
                Criteria.Last().IsLastGroup = true;
            }
        }

        private CriteriaPlaylist CreatePlaylist(CriteriaPlaylist playlist = null)
        {
            if (playlist == null)
            {
                playlist = new CriteriaPlaylist(0, "", false);
            }

            playlist.Title = PlaylistTitle;
            playlist.OrderByProperty = OrderByProperty;
            playlist.OrderByDescending = OrderByDescending;
            playlist.MaxTracks = MaxTracks;

            playlist.CriteriaGroups.Clear();

            foreach (var group in Criteria.Where(g => g.Criteria.Any()))
            {
                var criteriaGroup = new CriteriaGroup();

                foreach (var criteria in group.Criteria.Where(g => g.PropertyName != null))
                {
                    switch (criteria.PropertyOwner)
                    {
                        case PropertyOwner.Track:
                            criteriaGroup.TrackCriteria.Add(_trackCriteriaService.GetTrackCriteria(criteria.PropertyName.Value, criteria.CriteriaType.Value, criteria.ValueString));
                            break;
                        case PropertyOwner.Disc:
                            criteriaGroup.DiscCriteria.Add(_trackCriteriaService.GetDiscCriteria(criteria.PropertyName.Value, criteria.CriteriaType.Value, criteria.ValueString));
                            break;
                        case PropertyOwner.Album:
                            criteriaGroup.AlbumCriteria.Add(_trackCriteriaService.GetAlbumCriteria(criteria.PropertyName.Value, criteria.CriteriaType.Value, criteria.ValueString));
                            break;
                        case PropertyOwner.Artist:
                            criteriaGroup.ArtistCriteria.Add(_trackCriteriaService.GetArtistCriteria(criteria.PropertyName.Value, criteria.CriteriaType.Value, criteria.ValueString));
                            break;
                        default:
                            throw new InvalidOperationException("Invalid property owner");
                    }
                }

                playlist.CriteriaGroups.Add(criteriaGroup);
            }

            return playlist;
        }

        public List<string> Tags => _tags.Value;
        public List<string> Cities => _cities.Value;
        public List<string> States => _states.Value;
        public List<string> Countries => _countries.Value;
        public List<string> Groupings => _groupings.Value;
    }
}
