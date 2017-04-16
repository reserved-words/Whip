using GalaSoft.MvvmLight.Command;
using System;
using Whip.Common;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public abstract class EditableTabViewModelBase : TabViewModelBase
    {
        private bool _modified;

        public EditableTabViewModelBase(TabType tabType, IconType icon, string title, bool visible = true)
            :base(tabType, icon, title, visible)
        {
            CancelCommand = new RelayCommand(OnCancelAndFinish);
            SaveCommand = new RelayCommand(OnSaveAndFinish);
        }

        public event Action FinishedEditing;

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

        public bool Modified
        {
            get { return _modified; }
            set { Set(ref _modified, value); }
        }

        private void OnCancelAndFinish()
        {
            OnCancel();
            OnFinish();
        }

        public abstract void OnCancel();

        private void OnFinish()
        {
            Modified = false;
            FinishedEditing?.Invoke();
        }

        private void OnSaveAndFinish()
        {
            OnSave();
            OnFinish();
        }

        public abstract void OnSave();

        protected void SetModified<T>(ref T property, T value)
        {
            Set(ref property, value);
            Modified = true;
        }
    }
}
