using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.ComponentModel;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.Windows
{
    public class EnterStringViewModel : DialogViewModel, IDataErrorInfo
    {
        private readonly Func<string, bool> _isValid;
        private readonly string _errorMessage;
        private readonly IMessenger _messenger;

        private string _result;
        
        public EnterStringViewModel(IMessenger messenger, string title, string text, Func<string, bool> isValid = null, string errorMessage = null)
            :base(messenger, title)
        {
            Text = text;

            _isValid = isValid;
            _errorMessage = errorMessage;
            _messenger = messenger;

            OkCommand = new RelayCommand(OnOk);
            CancelCommand = new RelayCommand(OnCancel);
        }

        public string Result
        {
            get { return _result; }
            set { Set(ref _result, value); }
        }

        public string Text { get; private set; }
        public Func<string, bool> IsValid { get; private set; }

        public RelayCommand OkCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public string Error => this[nameof(Result)];

        public string this[string columnName] => columnName != nameof(Result) || _isValid == null || _isValid(Result) 
            ? string.Empty
            : _errorMessage ?? "The value entered is not valid";

        private void OnOk()
        {
            if (!string.IsNullOrEmpty(Error))
            {
                _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Error, "Validation Error", Error));
                return;
            }

            Close();
        }

        private void OnCancel()
        {
            Result = null;
            Close();
        }
    }
}
