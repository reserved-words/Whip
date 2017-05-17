using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using Whip.Common;
using Whip.ViewModels.Messages;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public abstract class EditableTabViewModelBase : TabViewModelBase
    {
        private readonly IMessenger _messenger;

        private bool _modified;

        public EditableTabViewModelBase(TabType tabType, IconType icon, string title, IMessenger messenger, bool visible = true)
            :base(tabType, icon, title, visible)
        {
            _messenger = messenger;

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

        protected abstract string ErrorMessage { get; }

        protected abstract bool CustomCancel();

        public void OnFinish()
        {
            Modified = false;
            FinishedEditing?.Invoke(this);
        }

        private void OnSave()
        {
            if (!Validate())
                return;

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

        private bool Validate()
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                var errorMessage = string.Format("Please resolve the following validation errors:{0}{0}{1}", Environment.NewLine, ErrorMessage);
                _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Error, "Validation Error", errorMessage));
                return false;
            }

            return true;
        }
    }
}
