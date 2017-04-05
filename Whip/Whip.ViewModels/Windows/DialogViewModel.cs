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
        private string title;

        public DialogViewModel(string title)
        {
            Title = title;
        }

        public string Title
        {
            get { return title; }
            set { Set(ref title, value); }
        }
    }
}
