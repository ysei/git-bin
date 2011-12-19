﻿using System.IO;
using System.Linq;
using GitBin.Remotes;

namespace GitBin.Commands
{
    public class PushCommand : ICommand
    {
        private readonly ICacheManager _cacheManager;
        private readonly IRemote _remote;

        public PushCommand(
            ICacheManager cacheManager,
            IRemote remote,
            string[] args)
        {
            _cacheManager = cacheManager;
            _remote = remote;
        }

        public void Execute()
        {
            var filesInRemote = _remote.ListFiles();
            var filesInCache = _cacheManager.ListFiles();

            var filesToUpload = filesInCache.Except(filesInRemote).ToList();

            GitBinConsole.WriteLine("Uploading {0} chunks", filesToUpload.Count);

            foreach (var file in filesToUpload)
            {
                _remote.UploadFile(_cacheManager.GetPathForFile(file), file);
            }
        }
    }
}