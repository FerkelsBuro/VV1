using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    class WatchedDirectory
    {
        public Dictionary<string, WatchedFile > watchedFiles { get; set; }

        public void Update(FileEvent ev)
        {

        }
    }
}
