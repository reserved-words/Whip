using Ninject.Modules;
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

namespace Whip.Ioc
{
    public class IocConfiguration : NinjectModule
    {
        public override void Load()
        {
            RegisterSingletons();
            RegisterServices();
            RegisterMessageHandlers();
            RegisterPlayer();
            RegisterLastFmComponents();
        }

        private void RegisterSingletons()
        {
            Bind<Library>().ToSelf().InSingletonScope();
            Bind<IPlaylist>().To<Playlist>().InSingletonScope();
            Bind<IMessenger>().To<Messenger>().InSingletonScope();
            Bind<IUserSettings>().To<UserSettings>().InSingletonScope();
            Bind<ILastFmApiClientService>().To<LastFmApiClientService>().InSingletonScope();
        }

        private void RegisterServices()
        {
            Bind<ILoggingService>().To<LoggingService>().InTransientScope();
            Bind<ITaggingService>().To<TagLibService>().InTransientScope();
            Bind<IFileDialogService>().To<FileDialogService>().InTransientScope();
            Bind<IFolderDialogService>().To<FolderDialogService>().InTransientScope();

            Bind<IExceptionHandlingService>().To<ExceptionHandlingService>().InTransientScope();
            Bind<IFileService>().To<FileService>().InTransientScope();
            Bind<ILibraryService>().To<LibraryService>().InTransientScope();
            Bind<ILibraryDataOrganiserService>().To<LibraryDataOrganiserService>().InTransientScope();
            Bind<IDataPersistenceService>().To<XmlDataPersistenceService>().InTransientScope();
            Bind<ICommentProcessingService>().To<CommentProcessingService>().InTransientScope();
            Bind<ITrackFilterService>().To<TrackFilterService>().InTransientScope();
            Bind<IScrobblingRulesService>().To<ScrobblingRulesService>().InTransientScope();
            Bind<ILibrarySortingService>().To<LibrarySortingService>().InTransientScope();
            Bind<ITrackUpdateService>().To<TrackUpdateService>().InTransientScope();
            Bind<IImageProcessingService>().To<ImageProcessingService>().InTransientScope();
            Bind<IWebBrowserService>().To<WebBrowserService>().InTransientScope();
            Bind<IArchiveService>().To<ArchiveService>().InTransientScope();

            Bind<IAsyncMethodInterceptor>().To<WebMethodInterceptor>().InTransientScope();
        }

        private void RegisterLastFmComponents()
        {
            Bind<ISessionService>().To<SessionService>().InTransientScope();

            RegisterErrorHandlingLastFmService<LastFmApi.Interfaces.IScrobblingService, LastFmApi.ScrobblingService, Services.Interfaces.IScrobblingService, ErrorHandlingScrobblingService, LastFm.ScrobblingService>();
            RegisterErrorHandlingLastFmService<LastFmApi.Interfaces.ITrackLoveService, LastFmApi.TrackLoveService, Services.Interfaces.ITrackLoveService, ErrorHandlingTrackLoveService, LastFm.TrackLoveService>();
            RegisterErrorHandlingLastFmService<IArtistInfoService, LastFmApi.ArtistInfoService, IWebArtistInfoService, ErrorHandlingArtistInfoService, LastFm.ArtistInfoService>();
            RegisterErrorHandlingLastFmService<IAlbumInfoService, LastFmApi.AlbumInfoService, IWebAlbumInfoService, ErrorHandlingAlbumInfoService, LastFm.AlbumInfoService>();
        }

        private void RegisterErrorHandlingLastFmService<ILastFmService,LastFmService,IService,ErrorHandlingService,Service>()
            where LastFmService : ILastFmService
            where Service : IService
            where ErrorHandlingService : IService
        {
            Bind<ILastFmService>().To<LastFmService>().InTransientScope();

            Bind<IService>()
                .To<ErrorHandlingService>()
                .InTransientScope()
                .WithConstructorArgument(typeof(IService), ctx => ctx.Kernel.Get<Service>())
                .WithConstructorArgument(typeof(IAsyncMethodInterceptor), ctx => ctx.Kernel.Get<LastFmMethodInterceptor>());
        }

        private void RegisterMessageHandlers()
        {
            Bind<DialogMessageHandler>().ToSelf().InSingletonScope();
            Bind<PlayerCoordinator>().ToSelf().InSingletonScope();
            Bind<PlayRequestHandler>().ToSelf().InSingletonScope();
            Bind<TrackChangeCoordinator>().ToSelf().InSingletonScope();
            Bind<LibraryHandler>().ToSelf().InSingletonScope();
            Bind<ShowTabRequestHandler>().ToSelf().InSingletonScope();

            Bind<IPlayerUpdate>().ToMethod(ctx => ctx.Kernel.Get<TrackChangeCoordinator>());
            Bind<IShowTabRequestHandler>().ToMethod(ctx => ctx.Kernel.Get<ShowTabRequestHandler>());
        }

        private void RegisterPlayer()
        {
            Bind<NewFilePlayer>().ToSelf()
                .InSingletonScope()
                .WithConstructorArgument<IPlayer>(new Player());

            Bind<IPlayer>().To<ScrobblingPlayer>()
                .InSingletonScope()
                .WithConstructorArgument(typeof(IPlayer), ctx => ctx.Kernel.Get<NewFilePlayer>());
        }
    }
}
