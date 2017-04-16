using Whip.ViewModels;

namespace Whip.Ioc
{
    public class ViewModelLocator
    {
        public MainWindowViewModel MainWindowViewModel => IocKernel.Get<MainWindowViewModel>();
    }
}
