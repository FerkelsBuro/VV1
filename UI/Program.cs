using Domain.Models;
using Domain.Services;
using Infrastructure;
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
        private const int SyncIntervall = 400;

        private static void Main(string[] args)
        {
            AddLogger();

            var queue = new BlockingCollection<FileEvent>();
            var watchedDirectory = new WatchedDirectory();

            Task.Run(() => ExecuteDirectoryWatcher(queue));
            Task.Run(() => ExecuteWatchedDirectory(watchedDirectory, queue));
            Task.Run(() => ExecuteSync(watchedDirectory, Console.OpenStandardOutput()));

            Task.Delay(1000 * 60 * 60 * 24).Wait();
            Console.ReadLine();
        }

        private static async Task ExecuteWatchedDirectory(WatchedDirectory watchedDirectory, BlockingCollection<FileEvent> queue)
        {
            while (true)
            {
                if (queue.TryTake(out var fileEvent))
                {
                    watchedDirectory.Update(fileEvent);
                }

                await Task.Delay(PollingIntervallInMilliseconds);
            }
        }

        private static async Task ExecuteSync(WatchedDirectory watchedDirectory, Stream stream)
        {
            while (true)
            {
                await Task.Delay(SyncIntervall);
                watchedDirectory.Sync(stream);
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

            var directoryWatcher = new DirectoryWatcher(strategy, new DateiLeser());
            await directoryWatcher.Watch(Environment.CurrentDirectory, PollingIntervallInMilliseconds);
        }

        private static void AddLogger()
        {
            Directory.CreateDirectory("log");
            Trace.Listeners.Add(new TextWriterTraceListener(@"log\file.log"));
            Trace.AutoFlush = true;
        }
    }
}