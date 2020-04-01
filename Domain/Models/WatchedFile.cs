using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class WatchedFile: IEqualityComparer<WatchedFile>
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

        public bool Equals(WatchedFile x, WatchedFile y)
        {
            if (x == y) return true;
            if (x == null || y == null) return false;
            return x.HasSamePath(y);
        }

        public override bool Equals(object obj)
        {
            var file = obj as WatchedFile;
            return file != null &&
                   DateiName == file.DateiName &&
                   Verzeichnis == file.Verzeichnis &&
                   ZeitStempel == file.ZeitStempel &&
                   Zustand == file.Zustand;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DateiName, Verzeichnis, ZeitStempel, Zustand);
        }

        public int GetHashCode(WatchedFile obj)
        {
            return obj.GetHashCode();
        }

        public bool HasSamePath(WatchedFile other)
        {
            return (Verzeichnis + DateiName).Equals(other.Verzeichnis + other.DateiName);
        }
    }
}
