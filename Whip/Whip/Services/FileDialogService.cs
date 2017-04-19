using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class FileDialogService : IFileDialogService
    {
        private readonly Dictionary<FileType, string> filters = new Dictionary<FileType, string>
        {
            { FileType.Images, "Image Files (*.bmp, *.jpg)|*.bmp;*.jpg" }
        };

        public string OpenFileDialog(FileType fileType)
        {
            var openFileDialog = new OpenFileDialog();

            string filter;

            if (filters.TryGetValue(fileType, out filter))
            {
                openFileDialog.Filter = filter;
            }

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            else
            {
                return null;
            }
        }
    }
}
