using GalaSoft.MvvmLight.Messaging;
using System;
using Whip.Common.Interfaces;
using Whip.Common.Singletons;
using Whip.Common.Utilities;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;
using Whip.ViewModels.Windows;

namespace Whip.ViewModels.MessageHandlers
{
    public class LibraryHandler : IStartable
    {
        private readonly Library _library;
        private readonly ILibraryService _libraryService;
        private readonly IMessenger _messenger;

        public LibraryHandler(Library library, IMessenger messenger, ILibraryService libraryService)
        {
            _library = library;
            _messenger = messenger;
            _libraryService = libraryService;
        }

        public void Start()
        {
            _messenger.Register<LibraryUpdateRequest>(this, OnLibraryUpdateRequest);
        }

        public void Stop()
        {
            _messenger.Unregister<LibraryUpdateRequest>(this, OnLibraryUpdateRequest);
        }

        private void OnLibraryUpdateRequest(LibraryUpdateRequest message)
        {
            var progressBarViewModel = new ProgressBarViewModel(_messenger, "Populating Library");
            var startProgressBarMessage = new ShowDialogMessage(progressBarViewModel);

            OnPopulateLibraryAsync(progressBarViewModel);

            _messenger.Send(startProgressBarMessage);
        }

        private async void OnPopulateLibraryAsync(ProgressBarViewModel progressBarViewModel)
        {
            var progressHandler = new Progress<ProgressArgs>(progressBarViewModel.Update);
            var stopProgressBarMessage = new HideDialogMessage(progressBarViewModel.Guid);

            _library.Update(await _libraryService.GetLibraryAsync(progressHandler));

            _messenger.Send(stopProgressBarMessage);
        }

    }
}
