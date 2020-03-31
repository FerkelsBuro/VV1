using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    class WatchedFile
    {
        public string DateiName { get; set; }
        public string Verzeichnis { get; set; }
        public DateTime ZeitStempel { get; set; }
        public Dateizustande Zustand { get; set; }
    }
}
