using System;
using System.Collections.Generic;
using System.IO;

namespace Infrastructure
{
    public class DateiLeser
    {
        public IEnumerable<string> ReadFiles(string directory)
        {
            return Directory.GetFiles(directory);
        }

        public DateTime GetFileTime(string file)
        {
            return File.GetLastWriteTime(file);
        }
    }
}