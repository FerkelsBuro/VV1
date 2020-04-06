using Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Domain.Services
{
    public class WatchedDirectory
    {
        private static readonly Dictionary<Dateizustande, Dictionary<Alphabet, Dateizustande>> _stateDictionary;

        static WatchedDirectory()
        {
            _stateDictionary = new Dictionary<Dateizustande, Dictionary<Alphabet, Dateizustande>>
            {
                [Dateizustande.CREATED] = new Dictionary<Alphabet, Dateizustande>
                {
                    [Alphabet.CREATE] = Dateizustande.CREATED,
                    [Alphabet.DELETE] = Dateizustande.GONE,
                    [Alphabet.MODIFY] = Dateizustande.CREATED,
                    [Alphabet.SYNC] = Dateizustande.INSYNC,
                },

                [Dateizustande.DELETED] = new Dictionary<Alphabet, Dateizustande>
                {
                    [Alphabet.CREATE] = Dateizustande.MODIFIED,
                    [Alphabet.DELETE] = Dateizustande.DELETED,
                    [Alphabet.MODIFY] = Dateizustande.MODIFIED,
                    [Alphabet.SYNC] = Dateizustande.GONE,
                },

                [Dateizustande.GONE] = new Dictionary<Alphabet, Dateizustande>
                {
                    [Alphabet.CREATE] = Dateizustande.CREATED,
                    [Alphabet.DELETE] = Dateizustande.GONE,
                    [Alphabet.MODIFY] = Dateizustande.GONE,
                    [Alphabet.SYNC] = Dateizustande.GONE,
                },

                [Dateizustande.INSYNC] = new Dictionary<Alphabet, Dateizustande>
                {
                    [Alphabet.CREATE] = Dateizustande.MODIFIED,
                    [Alphabet.DELETE] = Dateizustande.DELETED,
                    [Alphabet.MODIFY] = Dateizustande.MODIFIED,
                    [Alphabet.SYNC] = Dateizustande.INSYNC,
                },

                [Dateizustande.MODIFIED] = new Dictionary<Alphabet, Dateizustande>
                {
                    [Alphabet.CREATE] = Dateizustande.MODIFIED,
                    [Alphabet.DELETE] = Dateizustande.DELETED,
                    [Alphabet.MODIFY] = Dateizustande.MODIFIED,
                    [Alphabet.SYNC] = Dateizustande.INSYNC,
                },
            };
        }

        public Dictionary<string, WatchedFile> WatchedFiles { get; set; } = new Dictionary<string, WatchedFile>();

        public void Update(FileEvent evt)
        {
            if (!WatchedFiles.ContainsKey(evt.WatchedFile.DateiName))
            {
                WatchedFiles.Add(evt.WatchedFile.DateiName, evt.WatchedFile);
                return;
            }

            var file = evt.WatchedFile;

            Trace.TraceInformation("Klasse {0}, Zeit {1}", nameof(WatchedDirectory), DateTime.Now);
            file.Zustand = _stateDictionary[file.Zustand][evt.Event];
        }

        public void Sync(Stream stream)
        {
            var message = JsonConvert.SerializeObject(WatchedFiles.ToList(), Formatting.Indented);

            var streamWrite = new StreamWriter(stream, new UnicodeEncoding());
            streamWrite.Write(message);
            streamWrite.Flush();

            foreach (var file in WatchedFiles.Values.ToList())
            {
                Update(new FileEvent(file, Alphabet.SYNC));
            }
        }
    }
}