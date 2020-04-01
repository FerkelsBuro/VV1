using Infrastructure;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
                .Select(f => new WatchedFile(Path.GetFileName(f), verzeichnis, dateiLeser.GetFileTime(f), Dateizustande.CREATED))
                .ToList();

            while(true)
            {
                List<WatchedFile> newFiles = dateiLeser.ReadFiles(verzeichnis)
                    .Select(f => new WatchedFile(Path.GetFileName(f), verzeichnis, dateiLeser.GetFileTime(f), Dateizustande.CREATED))
                    .ToList();
            
                foreach (WatchedFile newFile in newFiles)
                {
                    var Event = CheckFile(newFile);

                    Trace.TraceInformation(Event.DateiName + " " + Event.Event);
                }

                files = newFiles;
            }
        }

        private FileEvent CheckFile(WatchedFile file)
        {
            var searchedFile = files.Find(f => f.DateiName == file.DateiName);
            if (searchedFile == null)
            {
                return new FileEvent(file.DateiName, Alphabet.CREATE);
            }
            else if(files.Find(f => f.DateiName == file.DateiName && f.ZeitStempel == file.ZeitStempel) == null)
            {
                return new FileEvent(file.DateiName, Alphabet.MODIFY);
            }

            return new FileEvent(file.DateiName, Alphabet.SYNC);
        }
    }
}