using Ninject;
using Ninject.Modules;
using Whip.MessageHandlers;
using Whip.ViewModels.MessageHandlers;

namespace Whip.Ioc
{
    public static class IocKernel
    {
        private static StandardKernel _kernel;

        public static T Get<T>()
        {
            return _kernel.Get<T>();
        }

        public static void Initialize(params INinjectModule[] modules)
        {
            if (_kernel == null)
            {
                _kernel = new StandardKernel(modules);
            }
        }

        public static void StartMessageHandlers()
        {
            _kernel.Get<DialogMessageHandler>().Start();
            _kernel.Get<PlayerCoordinator>().Start();
        }

        public static void StopMessageHandlers()
        {
            _kernel.Get<DialogMessageHandler>().Stop();
            _kernel.Get<PlayerCoordinator>().Stop();
        }
    }
}
