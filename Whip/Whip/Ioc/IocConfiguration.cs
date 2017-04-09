using GalaSoft.MvvmLight.Messaging;
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
        }

        private void BindMessageHandlers()
        {
            Bind<DialogMessageHandler>().ToSelf().InSingletonScope();
            Bind<PlayerCoordinator>().ToSelf().InSingletonScope();
            Bind<PlayRequestHandler>().ToSelf().InSingletonScope();
        }

        private void BindPlayer()
        {
            Bind<IPlayer>().To<ScrobblingPlayer>()
                .InSingletonScope()
                .WithConstructorArgument<IPlayer>(new Player());
        }
    }
}
