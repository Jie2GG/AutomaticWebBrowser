using System;
using System.IO;
using System.Windows;

using AutomaticWebBrowser.Domain.Boot;
using AutomaticWebBrowser.Domain.Boot.Models;
using AutomaticWebBrowser.Domain.Configuration;
using AutomaticWebBrowser.Domain.Configuration.Models;
using AutomaticWebBrowser.Domain.Log;
using AutomaticWebBrowser.Services;
using AutomaticWebBrowser.ViewModels;
using AutomaticWebBrowser.Views;

using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;

using Serilog;
using Serilog.Core;
using Serilog.Events;

using Unity;

namespace AutomaticWebBrowser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private AWConfig? config;

        protected override void OnStartup (StartupEventArgs e)
        {
            // 解析启动参数
            if (BootModule.TryParse (e.Args, out BootArgs? args) && args != null)
            {
                this.config = ConfigurationModule.Load (args!.ConfigPath);
            }

            base.OnStartup (e);
        }

        protected override void RegisterTypes (IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<Window, MainView> ();
            containerRegistry.RegisterSingleton<MainViewModel> ();

            containerRegistry.RegisterInstance (this.config!, nameof (AWConfig));
            containerRegistry.RegisterSingleton<LoggerService> ();
        }

        protected override Window CreateShell ()
        {
            return this.Container.Resolve<Window> ();
        }

    }
}
