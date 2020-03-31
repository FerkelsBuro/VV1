using Domain.Models;
using System;

namespace UI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var watcher = new DirectoryWatcher(@"C:\Users\ITB\source\repos\VV1");
            watcher.Watch();
        }
    }
}
