using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.ViewModels.TabViewModels;

namespace Whip.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(LibraryViewModel libraryViewModel)
        {
            LibraryViewModel = libraryViewModel;
        }

        public LibraryViewModel LibraryViewModel { get; private set; }
    }
}
