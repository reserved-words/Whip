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
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave);
        }

        public event Action<EditableTabViewModelBase> FinishedEditing;

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

        public virtual bool Modified
        {
            get { return _modified; }
            set { Set(ref _modified, value); }
        }

        public void OnCancel()
        {
            if (!CustomCancel())
                return;

            OnFinish();
        }

        protected abstract bool CustomCancel();

        public void OnFinish()
        {
            Modified = false;
            FinishedEditing?.Invoke(this);
        }

        private void OnSave()
        {
            if (!CustomSave())
                return;

            OnFinish();
        }

        protected abstract bool CustomSave();

        protected void SetModified<T>(string propertyName, ref T property, T value)
        {
            Set(propertyName, ref property, value);
            Modified = true;
        }
    }
}
