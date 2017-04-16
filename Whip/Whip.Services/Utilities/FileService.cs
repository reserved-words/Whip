﻿using System;
using System.IO;
using System.Linq;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using File = Whip.Common.Model.File;

namespace Whip.Services
{
    public class FileService : IFileService
    {
        public FilesWithStatus GetFiles(string directory, string[] extensions, DateTime lastUpdated)
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
    }
}