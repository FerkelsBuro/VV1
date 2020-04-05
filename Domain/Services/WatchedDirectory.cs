using Domain.Models;
using System.Collections.Generic;

namespace Domain.Services
{
    public class WatchedDirectory
    {
        public Dictionary<string, WatchedFile> watchedFiles { get; set; }

        public void Update(FileEvent ev)
        {
            if (!watchedFiles.TryGetValue(ev.DateiName, out var file)) return;

            switch (file.Zustand)
            {
                case Dateizustande.CREATED:
                    switch (ev.Event)
                    {
                        case Alphabet.CREATE:
                        case Alphabet.MODIFY:
                            file.Zustand = Dateizustande.CREATED;
                            break;

                        case Alphabet.DELETE:
                            file.Zustand = Dateizustande.GONE;
                            break;

                        case Alphabet.SYNC:
                            file.Zustand = Dateizustande.INSYNC;
                            break;
                    }
                    break;
            }
        }
    }
}