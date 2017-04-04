using System.Collections.Generic;
using System.IO;
using System.Linq;
using Whip.Services.Interfaces;
using File = Whip.Common.Model.File;

namespace Whip.Services
{
    public class FileService : IFileService
    {
        public ICollection<File> GetFiles(string directory, params string[] extensions)
        {
            var files = new List<File>();
            
            foreach (string filepath in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
            {
                var fileInfo = new FileInfo(filepath);

                if (extensions.Contains(fileInfo.Extension))
                {
                    files.Add(new File(
                        filepath.Remove(0, directory.Length + 1),
                        fileInfo.CreationTime,
                        fileInfo.LastWriteTime));
                }
            }

            return files;
        }
    }
}
