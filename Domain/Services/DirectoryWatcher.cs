using Core.Extensions;
using Domain.Models;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class DirectoryWatcher
    {
        private readonly Action<FileEvent> _strategy;
        private readonly IDateiLeser _dateiLeser;
        private IEnumerable<WatchedFile> _files = new List<WatchedFile>();

        public DirectoryWatcher(Action<FileEvent> strategy, IDateiLeser dateiLeser)
        {
            _strategy = strategy;
            this._dateiLeser = dateiLeser;
        }

        public async Task Watch(string verzeichnis, int pollingIntervallInMilliseconds)
        {
            while (true)
            {
                var newFiles = Enumerable.Empty<WatchedFile>();

                try
                {
                    newFiles = GetDirectoryFiles(verzeichnis);
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

                await Task.Delay(pollingIntervallInMilliseconds);
            }
        }

        private void ExecuteStrategy(Func<IEnumerable<WatchedFile>> algorithmus, Alphabet alphabet)
        {
            algorithmus()
                .Select(f => new FileEvent(f, alphabet))
                .ForEach(evt =>
                {
                    _strategy(evt);
                });
        }

        private IEnumerable<WatchedFile> GetDirectoryFiles(string verzeichnis)
        {
            return _dateiLeser.ReadFiles(verzeichnis)
                .Select(f => new WatchedFile(Path.GetFileName(f), verzeichnis, _dateiLeser.GetFileTime(f), Dateizustande.CREATED))
                .ToList();
        }
    }
}