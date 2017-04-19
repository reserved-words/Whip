namespace Whip.Services.Interfaces
{
    public enum FileType { Images }

    public interface IFileDialogService
    {
        string OpenFileDialog(FileType fileType);
    }
}
