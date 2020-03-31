using System;

namespace Domain.Models
{
    public class WatchedFile
    {
        public string FileName { get; set; }
        public string Directory { get; set; }
        public DateTime LastModified { get; set; }
        public FileState State { get; set; }
    }
}