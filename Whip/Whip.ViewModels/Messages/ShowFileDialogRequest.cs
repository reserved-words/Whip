using Whip.Services.Interfaces;

namespace Whip.ViewModels.Messages
{
    public class ShowFileDialogRequest
    {
        public ShowFileDialogRequest(FileType fileType)
        {
            FileType = fileType;
        }

        public FileType FileType { get; private set; }
        public string Result { get; set; }
    }
}
