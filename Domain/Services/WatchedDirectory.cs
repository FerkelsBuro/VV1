using Domain.Models;
using System.Collections.Generic;

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

        public Dictionary<string, WatchedFile> watchedFiles { get; set; }

        public void Update(FileEvent ev)
        {
            if (!watchedFiles.TryGetValue(ev.DateiName, out var file)) return;

            file.Zustand = _stateDictionary[file.Zustand][ev.Event];
        }
    }
}