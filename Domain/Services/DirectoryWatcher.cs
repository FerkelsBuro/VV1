using Core.Extensions;
using Domain.Models;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Domain.Services
{
    public class DirectoryWatcher
    {
        private readonly Action<FileEvent> _strategy;
        private readonly string _verzeichnis;
        private readonly DateiLeser _dateiLeser;
        private IEnumerable<WatchedFile> _files = new List<WatchedFile>();

        public DirectoryWatcher(Action<FileEvent> strategy, string verzeichnis, DateiLeser dateiLeser = null)
        {
            _strategy = strategy;
            this._verzeichnis = verzeichnis;
            this._dateiLeser = dateiLeser ?? new DateiLeser();
        }

        public void Watch()
        {
            while (true)
            {
                var newFiles = Enumerable.Empty<WatchedFile>();

                try
                {
                    newFiles = GetDirectoryFiles();
                }
                catch (Exception e)
                {
                    Trace.TraceError("Fehler {0}, {1}, {2}", DateTime.Now, e.Message, e.StackTrace);
                }

                var watchedFiles = new WatchedFiles(_files);
                // Datei erstellt
                ExecuteStrategy(() => watchedFiles.GetCreatedFiles(newFiles), Alphabet.CREATE);
                // Datei geupdated
                ExecuteStrategy(() => watchedFiles.GetUpdatedFiles(newFiles), Alphabet.MODIFY);
                // Datei gelöscht
                ExecuteStrategy(() => watchedFiles.GetDeletedFiles(newFiles), Alphabet.DELETE);

                _files = newFiles;
            }
        }

        private void ExecuteStrategy(Func<IEnumerable<WatchedFile>> algorithmus, Alphabet alphabet)
        {
            algorithmus()
                .Select(f => new FileEvent(f.DateiName, alphabet))
                .ForEach(evt =>
                {
                    _strategy(evt);
                });
        }

        private IEnumerable<WatchedFile> GetDirectoryFiles()
        {
            return _dateiLeser.ReadFiles(_verzeichnis)
                .Select(f => new WatchedFile(Path.GetFileName(f), _verzeichnis, _dateiLeser.GetFileTime(f), Dateizustande.CREATED))
                .ToList();
        }
    }
}