using Ninject.Modules;
using Whip.TagLibSharp;
using Whip.Services;
using Whip.Services.Interfaces;
using Whip.ViewModels;
using Whip.MessageHandlers;
using Whip.Common.Singletons;
using Whip.ViewModels.MessageHandlers;
using Whip.ViewModels.TabViewModels;
using Whip.Common.Interfaces;
using Whip.WmpPlayer;
using Whip.LastFm;
using LastFmApi.Interfaces;
using LastFmApi;
using Ninject;
using GalaSoft.MvvmLight.Messaging;
using Whip.NLog;
using Whip.Services.Interfaces.Singletons;
using Whip.ViewModels.TabViewModels.EditTrack;
using Whip.Services.Singletons;

namespace Whip.Ioc
{
    public class IocConfiguration : NinjectModule
    {
        public override void Load()
        {
            BindViewModels();

            BindSingletons();
            
            BindServices();

            BindMessageHandlers();

            BindPlayer();

            BindLastFmComponents();
        }

        private void BindViewModels()
        {
            Bind<MainWindowViewModel>().ToSelf().InTransientScope();
            Bind<MainViewModel>().ToSelf().InTransientScope();
            Bind<PlayerControlsViewModel>().ToSelf().InTransientScope();
            Bind<LibraryViewModel>().ToSelf().InTransientScope();
            Bind<TrackViewModel>().ToSelf().InTransientScope();
        }

        private void BindSingletons()
        {
            Bind<Library>().ToSelf().InSingletonScope();
            Bind<IPlaylist>().To<Playlist>().InSingletonScope();
            Bind<IMessenger>().To<Messenger>().InSingletonScope();
            Bind<IUserSettings>().To<UserSettings>().InSingletonScope();
        }

        private void BindServices()
        {
            Bind<ILoggingService>().To<LoggingService>().InTransientScope();
            Bind<IExceptionHandlingService>().To<ExceptionHandlingService>().InTransientScope();
            Bind<IFileService>().To<FileService>().InTransientScope();
            Bind<ILibraryService>().To<LibraryService>().InTransientScope();
            Bind<ILibraryDataOrganiserService>().To<LibraryDataOrganiserService>().InTransientScope();
            Bind<IDataPersistenceService>().To<XmlDataPersistenceService>().InTransientScope();
            Bind<ITaggingService>().To<TagLibService>().InTransientScope();
            Bind<ICommentProcessingService>().To<CommentProcessingService>().InTransientScope();
            Bind<ITrackFilterService>().To<TrackFilterService>().InTransientScope();
            Bind<IScrobblingRulesService>().To<ScrobblingRulesService>().InTransientScope();
            Bind<ILibrarySortingService>().To<LibrarySortingService>().InTransientScope();
            Bind<IFileDialogService>().To<FileDialogService>().InTransientScope();
            Bind<ITrackUpdateService>().To<TrackUpdateService>().InTransientScope();
            Bind<IImageProcessingService>().To<ImageProcessingService>().InTransientScope();
        }

        private void BindLastFmComponents()
        {
            Bind<ILastFmApiClientService>().To<LastFmApiClientService>().InTransientScope();
            Bind<ILastFmSessionService>().To<LastFmSessionService>().InTransientScope();

            Bind<AuthorizedScrobblingService>().ToSelf().InTransientScope();
            Bind<AuthorizedTrackLoveService>().ToSelf().InTransientScope();

            Bind<ILastFmScrobblingService>().To<AuthorizedScrobblingService>().InTransientScope();
            Bind<ILastFmTrackLoveService>().To<AuthorizedTrackLoveService>().InTransientScope();

            Bind<IScrobblingService>()
                .To<Services.ScrobblingService>()
                .InTransientScope()
                .WithConstructorArgument(typeof(IScrobblingService), ctx => ctx.Kernel.Get<LastFm.ScrobblingService>());

            Bind<ITrackLoveService>()
                .To<TrackLoveService>()
                .InTransientScope();

            Bind<IWebAlbumInfoService>().To<AlbumInfoService>().InTransientScope();
        }

        private void BindMessageHandlers()
        {
            Bind<DialogMessageHandler>().ToSelf().InSingletonScope();
            Bind<PlayerCoordinator>().ToSelf().InSingletonScope();
            Bind<PlayRequestHandler>().ToSelf().InSingletonScope();
            Bind<TrackChangeCoordinator>().ToSelf().InSingletonScope();
            Bind<LibraryHandler>().ToSelf().InSingletonScope();
            Bind<ShowTabRequestHandler>().ToSelf().InSingletonScope();
            Bind<FileDialogRequestHandler>().ToSelf().InSingletonScope();

            Bind<IPlayerUpdate>().ToMethod(ctx => ctx.Kernel.Get<TrackChangeCoordinator>());
            Bind<IShowTabRequestHandler>().ToMethod(ctx => ctx.Kernel.Get<ShowTabRequestHandler>());
        }

        private void BindPlayer()
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
