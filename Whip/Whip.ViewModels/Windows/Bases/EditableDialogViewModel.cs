using System;
using GalaSoft.MvvmLight.Messaging;
using Whip.Common;
using GalaSoft.MvvmLight.Command;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.Windows
{
    public abstract class EditableDialogViewModel : DialogViewModel
    {
        private readonly IMessenger _messenger;

        private bool _modified;

        public EditableDialogViewModel(IMessenger messenger, string title, IconType iconType, Action callback = null)
            :base(messenger, title, iconType, callback)
        {
            _messenger = messenger;

            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave);
        }

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

        public virtual bool Modified
        {
            get { return _modified; }
            set { Set(ref _modified, value); }
        }

        public bool Saved { get; private set; }

        public void OnCancel()
        {
            if (!CustomCancel())
                return;
            
            OnFinish(false);
        }

        protected abstract string ErrorMessage { get; }

        protected abstract bool CustomCancel();

        public void OnFinish(bool saved)
        {
            Saved = saved;
            Modified = false;
            Close();
        }

        private void OnSave()
        {
            if (!Validate())
                return;

            if (!CustomSave())
                return;

            OnFinish(true);
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
