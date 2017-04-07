using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Whip.Ioc;
using Whip.Properties;
using Whip.ViewModels.Messages;

namespace Whip
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IocKernel.Initialize(new IocConfiguration());
            IocKernel.StartMessageHandlers();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            IocKernel.StopMessageHandlers();

            base.OnExit(e);
        }
    }
}
