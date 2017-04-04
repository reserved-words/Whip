using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.ViewModels;

namespace Whip.Ioc
{
    public class ViewModelLocator
    {
        public MainWindowViewModel MainWindowViewModel => IocKernel.Get<MainWindowViewModel>();
        public MainViewModel MainViewModel => IocKernel.Get<MainViewModel>();
        public SidebarViewModel SidebarViewModel => IocKernel.Get<SidebarViewModel>();
    }
}
