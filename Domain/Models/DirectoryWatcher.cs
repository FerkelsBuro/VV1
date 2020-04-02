using Core.Extensions;
using Infrastructure;
using System;
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
        private IEnumerable<WatchedFile> files = new List<WatchedFile>();

        public DirectoryWatcher(string verzeichnis, DateiLeser dateiLeser = null)
        {
            this.verzeichnis = verzeichnis;
            this.dateiLeser = dateiLeser ?? new DateiLeser();
        }

        public void Watch()
        {
            while (true)
            {
                var newFiles = GetDirectoryFiles();

                var watchedFiles = new WatchedFiles(files);
                // Datei erstellt
                LogEvent(() => watchedFiles.GetCreatedFiles(newFiles), Alphabet.CREATE);
                // Datei geupdated
                LogEvent(() => watchedFiles.GetUpdatedFiles(newFiles), Alphabet.MODIFY);
                // Datei gelöscht
                LogEvent(() => watchedFiles.GetDeletedFiles(newFiles), Alphabet.DELETE);

                files = newFiles;
            }
        }

        private void LogEvent(Func<IEnumerable<WatchedFile>> algorithmus, Alphabet alphabet)
        {
            algorithmus()
                .Select(f => new FileEvent(f.DateiName, alphabet))
                .ForEach(evt => Trace.TraceInformation(evt.DateiName + " " + evt.Event));
        }

        private IEnumerable<WatchedFile> GetDirectoryFiles()
        {
            return dateiLeser.ReadFiles(verzeichnis)
                .Select(f => new WatchedFile(Path.GetFileName(f), verzeichnis, dateiLeser.GetFileTime(f), Dateizustande.CREATED));
        }
    }
}