using Domain.Models;
using System;

namespace UI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var watcher = new DirectoryWatcher(Environment.CurrentDirectory);
            watcher.Watch();
        }
    }
}