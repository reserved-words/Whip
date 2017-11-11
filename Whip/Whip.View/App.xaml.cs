using System;
using System.Windows;
using System.Windows.Threading;
using Whip.Ioc;
using Whip.Services.Interfaces;

namespace Whip
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            IocKernel.Initialize();
            IocKernel.StartMessageHandlers();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            GetLogger().Info("Application started");

            var settings = GetUserSettings();
            await settings.SetStartupDefaultsAsync();

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

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            GetExceptionHandler().Fatal(e.ExceptionObject as Exception);
        }

        private static ILoggingService GetLogger()
        {
            return IocKernel.Get<ILoggingService>();
        }

        private static IExceptionHandlingService GetExceptionHandler()
        {
            return IocKernel.Get<IExceptionHandlingService>();
        }

        private static IUserSettings GetUserSettings()
        {
            return IocKernel.Get<IUserSettings>();
        }
    }
}
