using Domain.Models;
using Domain.Services;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;

namespace UI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AddLogger();

            var queue = new BlockingCollection<FileEvent>();
            Action<FileEvent> strategy = (fileEvent) =>
            {
                Trace.TraceInformation(fileEvent.DateiName + " " + fileEvent.Event);
                queue.Add(fileEvent);
            };

            var directoryWatcher = new DirectoryWatcher(strategy, Environment.CurrentDirectory);
            directoryWatcher.Watch();

            var watchedDirectory = new WatchedDirectory();

            while (true)
            {
                if (queue.TryTake(out var fileEvent))
                {
                    watchedDirectory.Update(fileEvent);
                }
            }
        }

        private static void AddLogger()
        {
            Directory.CreateDirectory("log");
            Trace.Listeners.Add(new TextWriterTraceListener(@"log\file.log"));
            Trace.AutoFlush = true;
        }
    }
}