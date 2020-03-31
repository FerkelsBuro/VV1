using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
