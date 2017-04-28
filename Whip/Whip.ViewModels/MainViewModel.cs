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
        private readonly EditSettingsViewModel _settingsViewModel;

        private readonly TabViewModelBase _defaultViewModel;

        private TabViewModelBase _selectedTab;
        private TabViewModelBase _returnToTab;
        private Track _currentTrack;

        private bool _selectedTabSetByViewModel;

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
            EditSettingsViewModel settingsViewModel,
            EditTrackViewModel editTrackViewModel,
            IMessenger messenger,
            IShowTabRequestHandler showTabRequester)
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

            _editTrackViewModel = editTrackViewModel;
            _settingsViewModel = settingsViewModel;
            _messenger = messenger;

            _defaultViewModel = libraryViewModel;
            _returnToTab = _defaultViewModel;

            SetSelectedTab(_defaultViewModel);

            ChangeTabCommand = new RelayCommand<TabViewModelBase>(OnChangingTab, CanChangeTab);

            showTabRequester.ShowEditTrackTab += ShowEditTrackTab;
            showTabRequester.ShowSettingsTab += ShowSettingsTab;

            editTrackViewModel.FinishedEditing += OnFinishedEditing;
            settingsViewModel.FinishedEditing += OnFinishedEditing;
        }

        private void OnFinishedEditing(EditableTabViewModelBase sender)
        {
            sender.IsVisible = false;
            SetSelectedTab(_returnToTab);
        }

        private bool CanChangeTab(TabViewModelBase newTab)
        {
            if (_selectedTabSetByViewModel)
            {
                _selectedTabSetByViewModel = false;
                return true;
            }

            _returnToTab = newTab;

            var editable = _selectedTab as EditableTabViewModelBase;

            if (editable == null)
                return true;

            if (editable.Modified)
            {
                var confirmationViewModel = new ConfirmationViewModel(_messenger, UnsavedChangesTitle, UnsavedChangesText, ConfirmationViewModel.ConfirmationType.YesNo);

                _messenger.Send(new ShowDialogMessage(confirmationViewModel));

                if (!confirmationViewModel.Result)
                    return false;
            }

            editable.OnCancel();
            return true;
        }

        private void SetSelectedTab(TabViewModelBase tab)
        {
            _selectedTabSetByViewModel = true;
            SelectedTab = tab;
        }

        private void ShowSettingsTab()
        {
            _returnToTab = SelectedTab;
            _settingsViewModel.Reset();
            _settingsViewModel.IsVisible = true;
            SetSelectedTab(_settingsViewModel);
        }

        private void ShowEditTrackTab(Track track)
        {
            _returnToTab = SelectedTab;
            _editTrackViewModel.Edit(track);
            _editTrackViewModel.IsVisible = true;
            SetSelectedTab(_editTrackViewModel);
        }

        private void OnChangingTab(TabViewModelBase newTab) { }

        public List<TabViewModelBase> Tabs { get; private set; }

        public RelayCommand<TabViewModelBase> ChangeTabCommand { get; private set; }

        public TabViewModelBase SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                if (_selectedTab == value)
                    return;

                _selectedTab?.OnHide();
                value.OnShow(_currentTrack);
                Set(ref _selectedTab, value);
            }
        }

        public void OnCurrentTrackChanged(Track track)
        {
            _currentTrack = track;
            SelectedTab.OnCurrentTrackChanged(track);
        }
        
        public void SelectTab(TabType key)
        {
            SetSelectedTab(Tabs.Single(t => t.Key == key));
        }
    }
}
