using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Whip.ViewModels.TabViewModels.Playlists
{
    public class CriteriaGroupViewModel : ViewModelBase
    {
        private bool _isLastGroup;

        public CriteriaGroupViewModel()
        {
            Criteria = new ObservableCollection<CriteriaViewModel>(new List<CriteriaViewModel>
            {
                new CriteriaViewModel()
            });

            AddNewClauseCommand = new RelayCommand(OnAddNewClause);
            RemoveClauseCommand = new RelayCommand<CriteriaViewModel>(OnRemoveClause);
        }

        public CriteriaGroupViewModel(List<CriteriaViewModel> criteria)
        {
            Criteria = new ObservableCollection<CriteriaViewModel>(criteria);

            AddNewClauseCommand = new RelayCommand(OnAddNewClause);
            RemoveClauseCommand = new RelayCommand<CriteriaViewModel>(OnRemoveClause);
        }

        private void OnAddNewClause()
        {
            Criteria.Add(new CriteriaViewModel());
        }

        private void OnRemoveClause(CriteriaViewModel clause)
        {
            Criteria.Remove(clause);
        }

        public ObservableCollection<CriteriaViewModel> Criteria { get; private set; }

        public RelayCommand AddNewClauseCommand { get; private set; }
        public RelayCommand<CriteriaViewModel> RemoveClauseCommand { get; private set; }

        public bool IsLastGroup
        {
            get { return _isLastGroup; }
            set { Set(ref (_isLastGroup), value); }
        }
    }
}
