using Domain.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
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

        public Dictionary<string, WatchedFile> WatchedFiles { get; set; }

        public void Update(FileEvent ev)
        {
            if (!WatchedFiles.TryGetValue(ev.DateiName, out var file)) return;

            file.Zustand = _stateDictionary[file.Zustand][ev.Event];
        }

        public void Sync(Stream stream)
        {
            var message = JsonConvert.SerializeObject(WatchedFiles);

            var streamWrite = new StreamWriter(stream, new UnicodeEncoding());
            streamWrite.Write(message);
            streamWrite.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            foreach (var file in WatchedFiles.Values)
            {
                Update(new FileEvent(file.DateiName, Alphabet.SYNC));
            }
        }
    }
}