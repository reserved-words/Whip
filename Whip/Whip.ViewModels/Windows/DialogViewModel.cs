using GalaSoft.MvvmLight;
using System;

namespace Whip.ViewModels.Windows
{
    public class DialogViewModel : ViewModelBase
    {
        private Guid _guid;
        private string _title;
        private Action _callback;

        public DialogViewModel(string title, Action callback = null)
        {
            Title = title;
            _callback = callback;
            _guid = Guid.NewGuid();
        }

        public Guid Guid => _guid;

        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        protected virtual void Close()
        {
            _callback?.Invoke();
        }
    }
}
