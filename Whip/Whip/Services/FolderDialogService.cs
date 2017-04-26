using Ookii.Dialogs.Wpf;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class FolderDialogService : IFolderDialogService
    {
        public string OpenFolderDialog(string startPath)
        {
            var openDialog = new VistaFolderBrowserDialog
            {
                SelectedPath = startPath
            };

            return openDialog.ShowDialog() == true
                ? openDialog.SelectedPath
                : null;
        }
    }
}
