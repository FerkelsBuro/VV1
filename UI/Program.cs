using Domain.Services;
using System;
using System.Diagnostics;
using System.IO;

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
            Directory.CreateDirectory("log");
            Trace.Listeners.Add(new TextWriterTraceListener(@"log\file.log"));
            Trace.AutoFlush = true;
        }
    }
}