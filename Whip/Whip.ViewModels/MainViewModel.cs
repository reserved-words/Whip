using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.ViewModels.Messages;
using Whip.ViewModels.TabViewModels;
using Whip.ViewModels.Utilities;
using Whip.ViewModels.Windows;

namespace Whip.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private const string UnsavedChangesTitle = "Unsaved Changes";
        private const string UnsavedChangesText = "There are unsaved changes on this tab. Are you happy to cancel these changes?";

        private readonly IMessenger _messenger;
        private readonly EditTrackViewModel _editTrackViewModel;
        private readonly TabViewModelBase _defaultViewModel;

        private TabViewModelBase _selectedTab;
        private Track _currentTrack;

        public MainViewModel(
            DashboardViewModel dashboardViewModel,
            LibraryViewModel libraryViewModel,
            PlaylistsViewModel playlistsViewModel, 
            CurrentPlaylistViewModel currentPlaylistViewModel,
            CurrentTrackViewModel currentTrackViewModel,
            CurrentArtistViewModel currentArtistViewModel,
            LastFmViewModel lastFmViewModel,
            NewsViewModel newsViewModel,
            SearchViewModel searchViewModel,
            ArchiveViewModel archiveViewModel,
            SettingsViewModel settingsViewModel,
            EditTrackViewModel editTrackViewModel,
            IMessenger messenger,
            IEditTrackRequester editTrackRequester)
        {
            Tabs = new List<TabViewModelBase>
            {
                dashboardViewModel,
                libraryViewModel,
                playlistsViewModel,
                currentTrackViewModel,
                currentArtistViewModel,
                currentPlaylistViewModel,
                lastFmViewModel,
                newsViewModel,
                searchViewModel,
                archiveViewModel,
                settingsViewModel,
                editTrackViewModel
            };

            _defaultViewModel = libraryViewModel;
            _editTrackViewModel = editTrackViewModel;
            _messenger = messenger;

            SelectedTab = libraryViewModel;

            ChangeTabCommand = new RelayCommand(OnChangingTab, CanChangeTab);

            editTrackRequester.RequestAccepted += EditTrackRequester_RequestAccepted;
            editTrackViewModel.FinishedEditing += OnFinishedEditing;
            settingsViewModel.FinishedEditing += OnFinishedEditing;
        }

        private void OnFinishedEditing()
        {
            SelectedTab = _defaultViewModel;
        }

        private bool CanChangeTab()
        {
            var editable = _selectedTab as EditableTabViewModelBase;

            if (editable == null || !editable.Modified)
                return true;

            var confirmationViewModel = new ConfirmationViewModel(_messenger, UnsavedChangesTitle, UnsavedChangesText,ConfirmationViewModel.ConfirmationType.YesNo);

            _messenger.Send(new ShowDialogMessage(confirmationViewModel));

            if (!confirmationViewModel.Result)
                return false;

            editable.OnCancel();
            return true;
        }

        private void EditTrackRequester_RequestAccepted(Track track)
        {
            _editTrackViewModel.Edit(track);
            SelectedTab = _editTrackViewModel;
        }

        private void OnChangingTab() { }

        public List<TabViewModelBase> Tabs { get; private set; }

        public RelayCommand ChangeTabCommand { get; private set; }

        public TabViewModelBase SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                value.OnCurrentTrackChanged(_currentTrack);
                Set(ref _selectedTab, value);
            }
        }

        public void OnCurrentTrackChanged(Track track)
        {
            _currentTrack = track;
            SelectedTab.OnCurrentTrackChanged(track);
        }

        public void OnEditTrack(Track track)
        {
            _editTrackViewModel.IsVisible = true;
        }

        public void SelectTab(TabType key)
        {
            SelectedTab = Tabs.Single(t => t.Key == key);
        }
    }
}
