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
            _kernel.Get<PlayRequestHandler>().Start();
            _kernel.Get<TrackChangeCoordinator>().Start();
            _kernel.Get<LibraryHandler>().Start();
            _kernel.Get<ShowTabRequestHandler>().Start();
            _kernel.Get<FileDialogRequestHandler>().Start();
        }

        public static void StopMessageHandlers()
        {
            _kernel.Get<DialogMessageHandler>().Stop();
            _kernel.Get<PlayerCoordinator>().Stop();
            _kernel.Get<PlayRequestHandler>().Stop();
            _kernel.Get<TrackChangeCoordinator>().Stop();
            _kernel.Get<LibraryHandler>().Stop();
            _kernel.Get<ShowTabRequestHandler>().Stop();
            _kernel.Get<FileDialogRequestHandler>().Stop();
        }
    }
}
