using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using File = Whip.Common.Model.File;
using static Whip.Common.Resources;

namespace Whip.Services
{
    public class FileService : IFileService
    {
        private readonly ICurrentDateTime _currentDateTime;

        public FileService(ICurrentDateTime currentDateTime)
        {
            _currentDateTime = currentDateTime;
        }

        public FilesWithStatus GetFiles(string directory, List<string> extensions, DateTime lastUpdated)
        {
            var files = new FilesWithStatus();

            foreach (string filepath in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
            {
                var fileInfo = new FileInfo(filepath);

                if (extensions.Contains(fileInfo.Extension))
                {
                    var relativeFilepath = filepath.Remove(0, directory.Length + 1);

                    if (fileInfo.LastWriteTime > lastUpdated || fileInfo.CreationTime > lastUpdated)
                    {
                        files.AddedOrModified.Add(new File(
                            filepath,
                            relativeFilepath,
                            fileInfo.CreationTime,
                            fileInfo.LastWriteTime));
                    }
                    else
                    {
                        files.ToKeep.Add(relativeFilepath);
                    }
                }
            }

            return files;
        }

        public List<File> GetAllFiles(string directory, List<string> extensions)
        {
            var files = new List<File>();

            foreach (var filepath in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
            {
                var fileInfo = new FileInfo(filepath);

                if (extensions.Contains(fileInfo.Extension))
                {
                    var relativeFilepath = filepath.Remove(0, directory.Length + 1);

                    files.Add(new File(
                        filepath,
                        relativeFilepath,
                        fileInfo.CreationTime,
                        fileInfo.LastWriteTime));
                }
            }

            return files;
        }
        
        public string CopyFile(string copyFromPath, string copyToDirectoryName)
        {
            var copyToDirectory = GetDirectoryPath(copyToDirectoryName);

            if (!Directory.Exists(copyToDirectory))
            {
                Directory.CreateDirectory(copyToDirectory);
            }

            var copyFileName = string.Format("copy_{0}{1}", _currentDateTime.Get().Ticks, Path.GetExtension(copyFromPath));
            var copyToPath = Path.Combine(copyToDirectory, copyFileName);

            System.IO.File.Copy(copyFromPath, copyToPath);
            System.IO.File.SetAttributes(copyToPath, FileAttributes.Normal);

            return copyToPath;
        }

        public void DeleteFiles(string directoryName, params string[] excludeFiles)
        {
            var directoryInfo = new DirectoryInfo(GetDirectoryPath(directoryName));

            directoryInfo
                .EnumerateFiles()
                .Where(f => !excludeFiles.Contains(f.Name))
                .ToList()
                .ForEach(f =>
                {
                    f.Attributes = FileAttributes.Normal;
                    f.Delete();
                });
        }

        public string[] GetFileContent(string directoryName, string filename)
        {
            var fullPath = Path.Combine(directoryName, filename);
            return System.IO.File.Exists(fullPath) 
                ? System.IO.File.ReadAllLines(fullPath)
                : null;
        }

        private string GetDirectoryPath(string directoryName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationTitle, directoryName);
        }
    }
}
