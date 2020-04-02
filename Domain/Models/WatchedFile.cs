using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class WatchedFile : IEqualityComparer<WatchedFile>
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
            return x.Equals(y);
        }

        public override bool Equals(object obj)
        {
            var file = obj as WatchedFile;
            return file != null &&
                   HasSamePath(file);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DateiName, Verzeichnis);
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