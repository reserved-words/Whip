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
using GalaSoft.MvvmLight.Messaging;
using Whip.NLog;
using Whip.Services.Interfaces.Singletons;
using Whip.Services.Singletons;
using Whip.LastFm.ErrorHandlingDecorators;
using Whip.View;
using Whip.XmlDataAccess;

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
            kernel.RegisterSingleton<Library>()
                .RegisterSingleton<IPlaylist, Playlist>()
                .RegisterSingleton<IMessenger, Messenger>()
                .RegisterSingleton<IUserSettings, UserSettings>()
                .RegisterSingleton<ILastFmApiClientService, LastFmApiClientService>();

            return kernel;
        }

        private static IKernel RegisterServices(this IKernel kernel)
        {
            kernel.Register<ILoggingService, LoggingService>()
                .Register<ITaggingService, TagLibService>()
                .Register<IFileDialogService, FileDialogService>()
                .Register<IFolderDialogService, FolderDialogService>()
                .Register<IExceptionHandlingService, ExceptionHandlingService>()
                .Register<IFileService, FileService>()
                .Register<ILibraryService, LibraryService>()
                .Register<ILibraryDataOrganiserService, LibraryDataOrganiserService>()
                .Register<ICommentProcessingService, CommentProcessingService>()
                .Register<ITrackFilterService, TrackFilterService>()
                .Register<IScrobblingRulesService, ScrobblingRulesService>()
                .Register<ILibrarySortingService, LibrarySortingService>()
                .Register<ITrackUpdateService, TrackUpdateService>()
                .Register<IImageProcessingService, ImageProcessingService>()
                .Register<IWebBrowserService, WebBrowserService>()
                .Register<IArchiveService, ArchiveService>()
                .Register<IRssService, RssService>()
                .Register<ITrackSearchService, TrackSearchService>()
                .Register<IAsyncMethodInterceptor, WebMethodInterceptor>();

            return kernel;
        }

        private static IKernel RegisterRepositories(this IKernel kernel)
        {
            kernel.Register<ITrackRepository, TrackRepository>()
                .Register<IRssFeedsRepository, RssFeedsRepository>()
                .Register<IPlaylistRepository, PlaylistRepository>();

            return kernel;
        }

        private static IKernel RegisterLastFmComponents(this IKernel kernel)
        {
            kernel.Register<ISessionService, SessionService>();

            kernel.RegisterErrorHandlingLastFmService<LastFmApi.Interfaces.IScrobblingService, LastFmApi.ScrobblingService, Services.Interfaces.IScrobblingService, ErrorHandlingScrobblingService, LastFm.ScrobblingService>()
                .RegisterErrorHandlingLastFmService<LastFmApi.Interfaces.ITrackLoveService, LastFmApi.TrackLoveService, Services.Interfaces.ITrackLoveService, ErrorHandlingTrackLoveService, LastFm.TrackLoveService>()
                .RegisterErrorHandlingLastFmService<IArtistInfoService, LastFmApi.ArtistInfoService, IWebArtistInfoService, ErrorHandlingArtistInfoService, LastFm.ArtistInfoService>()
                .RegisterErrorHandlingLastFmService<IAlbumInfoService, LastFmApi.AlbumInfoService, IWebAlbumInfoService, ErrorHandlingAlbumInfoService, LastFm.AlbumInfoService>();

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
                .RegisterSingleton<PlayerCoordinator>()
                .RegisterSingleton<PlayRequestHandler>()
                .RegisterSingleton<TrackChangeCoordinator>()
                .RegisterSingleton<LibraryHandler>()
                .RegisterSingleton<ShowTabRequestHandler>();

            kernel.Bind<IPlayerUpdate>().ToMethod(ctx => ctx.Kernel.Get<TrackChangeCoordinator>());
            kernel.Bind<IShowTabRequestHandler>().ToMethod(ctx => ctx.Kernel.Get<ShowTabRequestHandler>());

            return kernel;
        }

        private static IKernel RegisterPlayer(this IKernel kernel)
        {
            kernel.Bind<NewFilePlayer>().ToSelf()
                .InSingletonScope()
                .WithConstructorArgument<IPlayer>(new Player());

            kernel.Bind<IPlayer>().To<ScrobblingPlayer>()
                .InSingletonScope()
                .WithConstructorArgument(typeof(IPlayer), ctx => ctx.Kernel.Get<NewFilePlayer>());

            return kernel;
        }

        private static IKernel Register<Service>(this IKernel kernel)
        {
            kernel.Bind<Service>().ToSelf().InTransientScope();
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
