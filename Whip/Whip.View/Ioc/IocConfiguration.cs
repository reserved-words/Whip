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
using Whip.ViewModels;
using Whip.TweetInvi;

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
                .RegisterSingleton<IConfigSettings, ConfigSettings>()
                .RegisterSingleton<ILastFmApiClientService, LastFmApiClientService>()
                .RegisterSingleton<TrackContextMenuViewModel, TrackContextMenuViewModel>();

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
                .Register<IPlayRequestHandler, PlayRequestHandler>()
                .Register<ITrackUpdateService, TrackUpdateService>()
                .Register<IImageProcessingService, ImageProcessingService>()
                .Register<IWebBrowserService, WebBrowserService>()
                .Register<IArchiveService, ArchiveService>()
                .Register<IRssService, RssService>()
                .Register<ITrackSearchService, TrackSearchService>()
                .Register<IAsyncMethodInterceptor, WebMethodInterceptor>()
                .Register<IWebHelperService, WebHelperService>()
                .Register<IVideoService, YouTubeVideoService>()
                .Register<IEventsService, BandsInTownArtistEventsService>()
                .Register<ITwitterService, TwitterService>()
                .Register<IArtistWebInfoService, ArtistWebInfoService>();

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

            kernel.RegisterErrorHandlingLastFmService<LastFmApi.Interfaces.IScrobblingService, 
                    LastFmApi.ScrobblingService, 
                    Services.Interfaces.IScrobblingService, 
                    ErrorHandlingScrobblingService, 
                    LastFm.ScrobblingService>()
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
                    LastFm.AlbumInfoService>();

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

        private static IKernel RegisterErrorHandlingWebService<IService, ErrorHandlingService, Service>(this IKernel kernel)
            where Service : IService
            where ErrorHandlingService : IService
        {
            kernel.Bind<IService>()
                .To<ErrorHandlingService>()
                .InTransientScope()
                .WithConstructorArgument(typeof(IService), ctx => ctx.Kernel.Get<Service>());

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
            kernel.Bind<NewFilePlayer>().ToSelf()
                .InSingletonScope()
                .WithConstructorArgument(typeof(IPlayer), ctx => ctx.Kernel.Get<Player>());

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
