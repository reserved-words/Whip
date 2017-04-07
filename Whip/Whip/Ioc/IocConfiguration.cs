using GalaSoft.MvvmLight.Messaging;
using Ninject.Modules;
using TagLibSharp;
using Whip.Services;
using Whip.Services.Interfaces;
using Whip.ViewModels;
using Whip.MessageHandlers;
using Whip.Common.Model;

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
        }

        private void BindViewModels()
        {
            Bind<MainWindowViewModel>().ToSelf().InTransientScope();
            Bind<MainViewModel>().ToSelf().InTransientScope();
            Bind<SidebarViewModel>().ToSelf().InTransientScope();
        }

        private void BindSingletons()
        {
            Bind<Library>().ToSelf().InSingletonScope();
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
        }

        private void BindMessageHandlers()
        {
            Bind<DialogMessageHandler>().ToSelf().InSingletonScope();
        }
    }
}
