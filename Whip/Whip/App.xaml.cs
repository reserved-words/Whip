using System.Windows;
using Whip.Ioc;
using Whip.Services.Interfaces.Utilities;

namespace Whip
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IocKernel.Initialize(new IocConfiguration());
            IocKernel.StartMessageHandlers();

            GetLogger().Info("Application started");

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            IocKernel.StopMessageHandlers();

            GetLogger().Info("Application stopped");

            base.OnExit(e);
        }

        private ILoggingService GetLogger()
        {
            return IocKernel.Get<ILoggingService>();
        }
    }
}
