using System.Collections.Generic;
using System.Linq;

namespace Domain.Models
{
    public class WatchedFiles
    {
        private readonly IEnumerable<WatchedFile> _watchedFiles;

        public WatchedFiles(IEnumerable<WatchedFile> watchedFiles)
        {
            _watchedFiles = watchedFiles;
        }

        public IEnumerable<WatchedFile> GetDeletedFiles(IEnumerable<WatchedFile> newFiles)
        {
            return _watchedFiles.Except(newFiles).ToList();
        }

        public IEnumerable<WatchedFile> GetUpdatedFiles(IEnumerable<WatchedFile> newFiles)
        {
            return newFiles.Where(change => _watchedFiles.Any(f => f.HasSamePath(change) && f.ZeitStempel != change.ZeitStempel)).ToList();
        }

        public IEnumerable<WatchedFile> GetCreatedFiles(IEnumerable<WatchedFile> newFiles)
        {
            return newFiles.Except(_watchedFiles).ToList();
        }
    }
}