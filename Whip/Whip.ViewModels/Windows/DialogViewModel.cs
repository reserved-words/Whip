using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.ViewModels.Windows
{
    public class DialogViewModel : ViewModelBase
    {
        private Guid _guid;
        private string _title;

        public DialogViewModel(string title)
        {
            Title = title;
            _guid = Guid.NewGuid();
        }

        public Guid Guid => _guid;

        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }
    }
}
