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
        }

        private void BindSingletons()
        {
            Bind<Library>().ToSelf().InSingletonScope();
            Bind<Playlist>().ToSelf().InSingletonScope();
            Bind<IMessenger>().To<Messenger>().InSingletonScope();
        }

        private void BindServices()
        {
            Bind<ILoggingService>().To<LoggingService>().InTransientScope();
            Bind<IExceptionHandlingService>().To<ExceptionHandlingService>().InTransientScope();
            Bind<IFileService>().To<FileService>().InTransientScope();
            Bind<ILibraryService>().To<LibraryService>().InTransientScope();
            Bind<IUserSettingsService>().To<UserSettingsService>().InTransientScope();
            Bind<ILibraryDataOrganiserService>().To<LibraryDataOrganiserService>().InTransientScope();
            Bind<IDataPersistenceService>().To<XmlDataPersistenceService>().InTransientScope();
            Bind<ITaggingService>().To<TagLibService>().InTransientScope();
            Bind<IDirectoryStructureService>().To<DirectoryStructureService>().InTransientScope();
            Bind<ICommentProcessingService>().To<CommentProcessingService>().InTransientScope();
            Bind<ITrackFilterService>().To<TrackFilterService>().InTransientScope();
            Bind<IScrobblingRulesService>().To<ScrobblingRulesService>().InTransientScope();
            Bind<ILibrarySortingService>().To<LibrarySortingService>().InTransientScope();
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
                .To<CachingScrobbleService>()
                .InTransientScope()
                .WithConstructorArgument(typeof(IScrobblingService), ctx => ctx.Kernel.Get<ScrobblingService>());

            Bind<ITrackLoveService>()
                .To<TrackLoveService>()
                .InTransientScope();
        }

        private void BindMessageHandlers()
        {
            Bind<DialogMessageHandler>().ToSelf().InSingletonScope();
            Bind<PlayerCoordinator>().ToSelf().InSingletonScope();
            Bind<PlayRequestHandler>().ToSelf().InSingletonScope();
            Bind<TrackChangeCoordinator>().ToSelf().InSingletonScope();
            Bind<LibraryHandler>().ToSelf().InSingletonScope();
            Bind<EditTrackRequestHandler>().ToSelf().InSingletonScope();

            Bind<IPlayerUpdate>().ToMethod(ctx => ctx.Kernel.Get<TrackChangeCoordinator>());
            Bind<IEditTrackRequester>().ToMethod(ctx => ctx.Kernel.Get<EditTrackRequestHandler>());
        }

        private void BindPlayer()
        {
            Bind<IPlayer>().To<ScrobblingPlayer>()
                .InSingletonScope()
                .WithConstructorArgument<IPlayer>(new Player());
        }
    }
}
