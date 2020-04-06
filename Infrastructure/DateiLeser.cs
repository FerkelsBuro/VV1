using System;
using System.Collections.Generic;
using System.IO;

namespace Infrastructure
{
    public interface IDateiLeser
    {
        DateTime GetFileTime(string file);

        IEnumerable<string> ReadFiles(string directory);
    }

    public class DateiLeser : IDateiLeser
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