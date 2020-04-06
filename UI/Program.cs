using Domain.Models;
using Domain.Services;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace UI
{
    internal class Program
    {
        private const int PollingIntervallInMilliseconds = 1000;

        private static void Main(string[] args)
        {
            AddLogger();

            var queue = new BlockingCollection<FileEvent>();
            Task.Run(() => ExecuteDirectoryWatcher(queue));
            Task.Run(() => ExecuteWatchedDirectory(queue));

            Console.ReadLine();
        }

        private static async Task ExecuteWatchedDirectory(BlockingCollection<FileEvent> queue)
        {
            var watchedDirectory = new WatchedDirectory();

            while (true)
            {
                if (queue.TryTake(out var fileEvent))
                {
                    watchedDirectory.Update(fileEvent);
                }

                await Task.Delay(PollingIntervallInMilliseconds);
            }
        }

        private static async Task ExecuteDirectoryWatcher(BlockingCollection<FileEvent> queue)
        {
            Action<FileEvent> strategy = (fileEvent) =>
            {
                Trace.TraceInformation("Klasse {0}, Zeit {1}", nameof(DirectoryWatcher), DateTime.Now);
                Trace.TraceInformation(fileEvent.WatchedFile + " " + fileEvent.Event);
                queue.Add(fileEvent);
            };

            var directoryWatcher = new DirectoryWatcher(strategy, Environment.CurrentDirectory);
            await directoryWatcher.Watch(PollingIntervallInMilliseconds);
        }

        private static void AddLogger()
        {
            Directory.CreateDirectory("log");
            Trace.Listeners.Add(new TextWriterTraceListener(@"log\file.log"));
            Trace.AutoFlush = true;
        }
    }
}