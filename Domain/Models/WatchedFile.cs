using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class WatchedFile
    {
        public WatchedFile(string dateiName, string verzeichnis, DateTime zeitStempel, Dateizustande zustand)
        {
            DateiName = dateiName;
            Verzeichnis = verzeichnis;
            ZeitStempel = zeitStempel;
            Zustand = zustand;
        }

        public string DateiName { get; set; }
        public string Verzeichnis { get; set; }
        public DateTime ZeitStempel { get; set; }
        public Dateizustande Zustand { get; set; }
    }
}
