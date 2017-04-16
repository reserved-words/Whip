using System;
using System.Windows;
using System.Windows.Threading;
using Whip.Ioc;
using Whip.Services.Interfaces;

namespace Whip
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IocKernel.Initialize(new IocConfiguration());
            IocKernel.StartMessageHandlers();

            GetLogger().Info("Application started");

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            IocKernel.StopMessageHandlers();

            var logger = GetLogger();
            logger.Info("Application stopped");
            logger.Info("-------------------");

            base.OnExit(e);
        }

        private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            GetExceptionHandler().Fatal(e.Exception);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            GetExceptionHandler().Fatal(e.ExceptionObject as Exception);
        }

        private ILoggingService GetLogger()
        {
            return IocKernel.Get<ILoggingService>();
        }

        private IExceptionHandlingService GetExceptionHandler()
        {
            return IocKernel.Get<IExceptionHandlingService>();
        }
    }
}
