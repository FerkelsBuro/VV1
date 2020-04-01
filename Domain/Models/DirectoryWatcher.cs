using Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Models
{
    public class DirectoryWatcher
    {
        private readonly string verzeichnis;
        private readonly DateiLeser dateiLeser;
        private List<WatchedFile> files = new List<WatchedFile>();

        public DirectoryWatcher(string verzeichnis, DateiLeser dateiLeser = null)
        {
            this.verzeichnis = verzeichnis;
            this.dateiLeser = dateiLeser ?? new DateiLeser();
        }

        public void Watch()
        {
            files = dateiLeser.ReadFiles(verzeichnis)
                .Select(f => new WatchedFile(f, verzeichnis, dateiLeser.GetFileTime(f), Dateizustande.CREATED))
                .ToList();
        }
    }
}