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

namespace Whip.ViewModels.TabViewModels
{
    public class EditCriteriaPlaylistViewModel : EditableTabViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly IPlaylistCriteriaService _playlistCriteriaService;
        private readonly IPlaylistRepository _repository;

        private CriteriaPlaylist _playlist;

        private string _playlistTitle;
        private PropertyName? _orderByProperty;
        private bool _orderByDescending;
        private int? _maxTracks;

        public EditCriteriaPlaylistViewModel(IMessenger messenger, IPlaylistRepository repository, IPlaylistCriteriaService playlistCriteriaService)
            :base(TabType.Playlists, IconType.Cog, "Edit Playlist", messenger, false)
        {
            _messenger = messenger;
            _playlistCriteriaService = playlistCriteriaService;
            _repository = repository;

            AddNewCriteriaGroupCommand = new RelayCommand(OnAddNewCriteriaGroup);
            RemoveGroupCommand = new RelayCommand<CriteriaGroupViewModel>(OnRemoveGroup);
        }

        public RelayCommand AddNewCriteriaGroupCommand { get; private set; }
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

        public ObservableCollection<CriteriaGroupViewModel> Criteria { get; private set; }

        protected override string ErrorMessage => string.Empty;

        protected override bool CustomCancel()
        {
            return true;
        }

        protected override bool CustomSave()
        {
            _playlist.Title = PlaylistTitle;
            _playlist.OrderByProperty = OrderByProperty;
            _playlist.OrderByDescending = OrderByDescending;
            _playlist.MaxTracks = MaxTracks;

            _playlist.CriteriaGroups.Clear();

            foreach (var group in Criteria.Where(g => g.Criteria.Any()))
            {
                var criteriaGroup = new CriteriaGroup();
                
                foreach (var criteria in group.Criteria.Where(g => g.PropertyName != null))
                {
                    switch (criteria.PropertyOwner)
                    {
                        case PropertyOwner.Track:
                            criteriaGroup.TrackCriteria.Add(_playlistCriteriaService.GetTrackCriteria(criteria.PropertyName.Value, criteria.CriteriaType.Value, criteria.ValueString));
                            break;
                        case PropertyOwner.Disc:
                            criteriaGroup.DiscCriteria.Add(_playlistCriteriaService.GetDiscCriteria(criteria.PropertyName.Value, criteria.CriteriaType.Value, criteria.ValueString));
                            break;
                        case PropertyOwner.Album:
                            criteriaGroup.AlbumCriteria.Add(_playlistCriteriaService.GetAlbumCriteria(criteria.PropertyName.Value, criteria.CriteriaType.Value, criteria.ValueString));
                            break;
                        case PropertyOwner.Artist:
                            criteriaGroup.ArtistCriteria.Add(_playlistCriteriaService.GetArtistCriteria(criteria.PropertyName.Value, criteria.CriteriaType.Value, criteria.ValueString));
                            break;
                        default:
                            throw new InvalidOperationException("Invalid property owner");
                    }
                }

                _playlist.CriteriaGroups.Add(criteriaGroup);
            }

            _repository.Save(_playlist);

            return true;
        }

        public void Edit(CriteriaPlaylist playlist)
        {
            _playlist = playlist;

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

            if (Criteria.Any())
            {
                Criteria.Last().IsLastGroup = true;
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

        private void OnRemoveGroup(CriteriaGroupViewModel group)
        {
            Criteria.Remove(group);

            if (Criteria.Any())
            {
                Criteria.Last().IsLastGroup = true;
            }
        }
    }
}
