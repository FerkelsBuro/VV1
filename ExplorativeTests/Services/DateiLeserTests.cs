using Infrastructure;
using System;
using System.IO;
using Xunit;

namespace ExplorativeTests.Services
{
    public class DateiLeserTests
    {
        [Fact]
        public void GetFileTime_Test()
        {
            var leser = new DateiLeser();
            var path = Path.Combine(Environment.CurrentDirectory, @"assets\ferkel.txt");
            var time = leser.GetFileTime(path);

            Assert.NotEqual(default(DateTime), time);
        }
    }
}