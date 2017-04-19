using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using Whip.Common.Interfaces;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;

namespace Whip.MessageHandlers
{
    public class FileDialogRequestHandler : IStartable
    {
        private readonly IFileDialogService _fileDialogService;
        private readonly IMessenger _messenger;

        private readonly Dictionary<Guid, Dialog> _dialogs = new Dictionary<Guid, Dialog>();

        public FileDialogRequestHandler(IMessenger messenger, IFileDialogService fileDialogService)
        {
            _fileDialogService = fileDialogService;
            _messenger = messenger;
        }

        public void Start()
        {
            _messenger.Register<ShowFileDialogRequest>(this, ShowFileDialog);
        }

        public void Stop()
        {
            _messenger.Unregister<ShowFileDialogRequest>(this, ShowFileDialog);
        }

        private void ShowFileDialog(ShowFileDialogRequest message)
        {
            message.Result = _fileDialogService.OpenFileDialog(message.FileType);
        }
    }
}
