using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTests.Models
{
    public class WatchedFilesTests
    {
        [Fact]
        public void GetCreatedFiles_WhenCalled_ReturnsTrue()
        {
            var oldFiles = GetRandomFiles(1);
            var changedFiles = oldFiles.Concat(GetRandomFiles(1));

            var diff = new WatchedFiles(oldFiles).GetCreatedFiles(changedFiles);

            Assert.Single(diff);
            Assert.Equal(diff.Single(), changedFiles.Last());
        }

        [Fact]
        public void GetUpdatedFiles_WhenCalled_ReturnsTrue()
        {
            var oldFiles = GetRandomFiles(2);
            var clones = oldFiles.Take(1).Select(f => Clone(f)).ToList();
            clones.ForEach(c => c.ZeitStempel = DateTime.Now);

            var diff = new WatchedFiles(oldFiles).GetUpdatedFiles(clones);

            Assert.Single(diff);
        }

        [Fact]
        public void GetDeletedFiles_WhenCalled_ReturnsTrue()
        {
            var oldFiles = GetRandomFiles(2);
            var changedFiles = oldFiles.Take(1).ToList();

            var diff = new WatchedFiles(oldFiles).GetDeletedFiles(changedFiles);

            Assert.Single(diff);
            Assert.Equal(diff.Single(), oldFiles.Last());
        }

        private WatchedFile GetRandomFile()
        {
            return new WatchedFile(Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                DateTime.Now,
                Dateizustande.CREATED);
        }

        private IEnumerable<WatchedFile> GetRandomFiles(int count)
        {
            return Enumerable.Range(0, count).Select(i => GetRandomFile()).ToList();
        }

        private WatchedFile Clone(WatchedFile original)
        {
            return new WatchedFile(original.DateiName,
                                   original.Verzeichnis,
                                   original.ZeitStempel,
                                   original.Zustand);
        }
    }
}