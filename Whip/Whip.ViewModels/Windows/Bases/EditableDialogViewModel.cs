using System;
using GalaSoft.MvvmLight.Messaging;
using Whip.Common;
using GalaSoft.MvvmLight.Command;
using Whip.ViewModels.Messages;
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Whip.ViewModels.Windows
{
    public abstract class EditableDialogViewModel : DialogViewModel, IDataErrorInfo
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

        public virtual string Error => this[string.Empty];

        public string this[string propertyName]
        {
            get
            {
                var properties = string.IsNullOrEmpty(propertyName)
                    ? GetType().GetProperties().Where(p => p.CustomAttributes.Any()).ToList()
                    : new List<PropertyInfo> { GetType().GetProperty(propertyName) };

                return string.Join(Environment.NewLine, properties.Select(p => GetErrorMessage(p)).Where(s => !string.IsNullOrEmpty(s)));
            }
        }

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
            if (!string.IsNullOrEmpty(Error))
            {
                var errorMessage = string.Format("Please resolve the following validation errors:{0}{0}{1}", Environment.NewLine, Error);
                _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Error, "Validation Error", errorMessage));
                return false;
            }

            return true;
        }

        private string GetErrorMessage(PropertyInfo property)
        {
            var validationResults = new List<ValidationResult>();
            var value = property.GetValue(this);
            var validationContext = new ValidationContext(this)
            {
                MemberName = property.Name
            };

            if (Validator.TryValidateProperty(value, validationContext, validationResults))
                return null;

            return validationResults.First().ErrorMessage;
        }
    }
}
