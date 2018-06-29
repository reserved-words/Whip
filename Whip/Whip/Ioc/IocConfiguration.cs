using Whip.TagLibSharp;
using Whip.Services;
using Whip.Services.Interfaces;
using Whip.MessageHandlers;
using Whip.Common.Singletons;
using Whip.ViewModels.MessageHandlers;
using Whip.Common.Interfaces;
using Whip.WmpPlayer;
using Whip.LastFm;
using LastFmApi.Interfaces;
using LastFmApi;
using Ninject;
using Ninject.Extensions.Conventions;
using GalaSoft.MvvmLight.Messaging;
using Whip.Common.TrackSorters;
using Whip.NLog;
using Whip.Services.Interfaces.Singletons;
using Whip.Services.Singletons;
using Whip.LastFm.ErrorHandlingDecorators;
using Whip.XmlDataAccess;
using Whip.ViewModels;
using Whip.TweetInvi;
using Whip.LyricApi;
using Whip.XmlDataAccess.Interfaces;

namespace Whip.Ioc
{
    public static class IocConfiguration
    {
        public static void RegisterComponents(this IKernel kernel)
        {
            kernel.RegisterSingletons()
                .RegisterServices()
                .RegisterRepositories()
                .RegisterMessageHandlers()
                .RegisterPlayer()
                .RegisterLastFmComponents();
        }

        private static IKernel RegisterSingletons(this IKernel kernel)
        {
            kernel.RegisterSingleton<IPlaylist, Playlist>()
                .RegisterSingleton<Library>()
                .RegisterSingleton<IPlayRequestHandler, PlayRequestHandler>()
                .RegisterSingleton<IMessenger, Messenger>()
                .RegisterSingleton<IUserSettings, UserSettings>()
                .RegisterSingleton<IWebServicesStatus, WebServicesStatus>()
                .RegisterSingleton<ILastFmApiClientService, LastFmApiClientService>()
                .RegisterSingleton<TrackContextMenuViewModel, TrackContextMenuViewModel>();

            return kernel;
        }

        private static IKernel RegisterServices(this IKernel kernel)
        {
            kernel.Bind(x => x.FromAssemblyContaining<FileDialogService>()
                .SelectAllClasses()
                .InNamespaceOf<FileDialogService>()
                .BindDefaultInterface());

            kernel.Bind(x => x.FromAssemblyContaining<FileService>()
                .SelectAllClasses()
                .InNamespaceOf<FileService>()
                .NotInNamespaceOf<Playlist>()
                .BindDefaultInterface());

            kernel.Register<ILoggingService, LoggingService>()
                .Register<ITaggingService, TagLibService>()
                .Register<ITwitterService, TwitterService>()
                .Register<IVideoService, YouTubeVideoService>()
                .Register<ILyricsService, LyricsService>()
                .Register<IEventsService, BandsInTownArtistEventsService>()
                .Register<IDefaultTrackSorter, DefaultTrackSorter>()
                .Register<IRandomTrackSorter, RandomTrackSorter>()
                .Register<IAsyncMethodInterceptor, WebMethodInterceptor>();

            return kernel;
        }

        private static IKernel RegisterRepositories(this IKernel kernel)
        {
            kernel.Register<ITrackXmlParser, TrackXmlParser>();

            kernel.Bind<IPlaylistRepository>()
                .To<PlaylistRepository>()
                .InTransientScope()
                .WithConstructorArgument(typeof(IXmlProvider), ctx => ctx.Kernel.Get<PlaylistXmlProvider>());

            kernel.Bind<ITrackRepository>()
                .To<TrackRepository>()
                .InTransientScope()
                .WithConstructorArgument(typeof(IXmlProvider), ctx => ctx.Kernel.Get<TrackXmlProvider>());

            kernel.Bind<IRssFeedsRepository>()
                .To<RssFeedsRepository>()
                .InTransientScope()
                .WithConstructorArgument(typeof(IXmlProvider), ctx => ctx.Kernel.Get<RssFeedsXmlProvider>());

            kernel.Bind<IConfigSettings>()
                .To<ConfigSettings>()
                .InSingletonScope()
                .WithConstructorArgument(typeof(IXmlProvider), ctx => ctx.Kernel.Get<ConfigXmlProvider>());

            return kernel;
        }

        private static IKernel RegisterLastFmComponents(this IKernel kernel)
        {
            kernel.Register<ISessionService, SessionService>();

            kernel.RegisterErrorHandlingLastFmService<IScrobblingService,
                    ScrobblingService,
                    IScrobbler,
                    ErrorHandlingScrobbler,
                    Scrobbler>()
                .RegisterErrorHandlingLastFmService<LastFmApi.Interfaces.ITrackLoveService,
                    LastFmApi.TrackLoveService,
                    Services.Interfaces.ITrackLoveService,
                    ErrorHandlingTrackLoveService,
                    LastFm.TrackLoveService>()
                .RegisterErrorHandlingLastFmService<LastFmApi.Interfaces.IArtistInfoService,
                    LastFmApi.ArtistInfoService,
                    Services.Interfaces.IArtistInfoService,
                    ErrorHandlingArtistInfoService,
                    LastFm.ArtistInfoService>()
                .RegisterErrorHandlingLastFmService<LastFmApi.Interfaces.IAlbumInfoService,
                    LastFmApi.AlbumInfoService,
                    Services.Interfaces.IAlbumInfoService,
                    ErrorHandlingAlbumInfoService,
                    LastFm.AlbumInfoService>()
                .RegisterErrorHandlingLastFmService<IUserInfoService,
                    LastFmApi.UserInfoService,
                    IPlayHistoryService,
                    ErrorHandlingUserInfoService,
                    LastFm.UserInfoService>();

            return kernel;
        }

        private static IKernel RegisterErrorHandlingLastFmService<ILastFmService,LastFmService,IService,ErrorHandlingService,Service>(this IKernel kernel)
            where LastFmService : ILastFmService
            where Service : IService
            where ErrorHandlingService : IService
        {
            kernel.Register<ILastFmService, LastFmService>();

            kernel.Bind<IService>()
                .To<ErrorHandlingService>()
                .InTransientScope()
                .WithConstructorArgument(typeof(IService), ctx => ctx.Kernel.Get<Service>())
                .WithConstructorArgument(typeof(IAsyncMethodInterceptor), ctx => ctx.Kernel.Get<LastFmMethodInterceptor>());

            return kernel;
        }

        private static IKernel RegisterMessageHandlers(this IKernel kernel)
        {
            kernel.RegisterSingleton<DialogMessageHandler>()
                .RegisterSingleton<LibraryHandler>()
                .RegisterSingleton<ShowTabRequestHandler>();

            kernel.Bind<IShowTabRequestHandler>().ToMethod(ctx => ctx.Kernel.Get<ShowTabRequestHandler>());

            return kernel;
        }

        private static IKernel RegisterPlayer(this IKernel kernel)
        {
            kernel.Bind<Player>().ToSelf()
                .InSingletonScope();

            kernel.Bind<NewFilePlayer>().ToSelf()
                .InSingletonScope()
                .WithConstructorArgument(typeof(IPlayer), ctx => ctx.Kernel.Get<Player>());

            kernel.Bind<IPlayer>().To<ScrobblingPlayer>()
                .InSingletonScope()
                .WithConstructorArgument(typeof(IPlayer), ctx => ctx.Kernel.Get<NewFilePlayer>());

            kernel.Bind<IPlayerVolume>().ToMethod(ctx => ctx.Kernel.Get<Player>());

            return kernel;
        }

        private static IKernel Register<IService, Service>(this IKernel kernel) where Service : IService
        {
            kernel.Bind<IService>().To<Service>().InTransientScope();
            return kernel;
        }

        private static IKernel RegisterSingleton<Service>(this IKernel kernel)
        {
            kernel.Bind<Service>().ToSelf().InSingletonScope();
            return kernel;
        }

        private static IKernel RegisterSingleton<IService, Service>(this IKernel kernel) where Service : IService
        {
            kernel.Bind<IService>().To<Service>().InSingletonScope();
            return kernel;
        }
    }
}
