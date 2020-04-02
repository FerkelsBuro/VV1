using Domain.Services;
using System;
using System.Diagnostics;

namespace UI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AddLogger();
            var watcher = new DirectoryWatcher(Environment.CurrentDirectory);
            watcher.Watch();
        }

        private static void AddLogger()
        {
            Trace.Listeners.Add(new TextWriterTraceListener("file.log"));
            Trace.AutoFlush = true;
        }
    }
}