using Domain.Models;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace UnitTests.Services
{
    public class WatchedDirectoryTests
    {
        private WatchedDirectory _sut;

        public WatchedDirectoryTests()
        {
            _sut = new WatchedDirectory();
        }

        [Fact]
        public void Sync_FileCreated_WritesToStream()
        {
            var file = GetFile("datei1", Dateizustande.CREATED);
            _sut.WatchedFiles = GetDictionary(file);

            using (var stream = new MemoryStream())
            {
                _sut.Sync(stream);
                Assert.NotEqual(0, stream.Length);
            }
        }

        [Fact]
        public void Sync_FileCreated_SyncsFile()
        {
            var file = GetFile("datei1", Dateizustande.CREATED);
            _sut.WatchedFiles = GetDictionary(file);

            using (var stream = new MemoryStream())
            {
                _sut.Sync(stream);
                Assert.Equal(Dateizustande.INSYNC, file.Zustand);
            }
        }

        [Fact]
        public void Sync_FileGone_FileStaysGone()
        {
            var file = GetFile("datei1", Dateizustande.GONE);
            _sut.WatchedFiles = GetDictionary(file);

            using (var stream = new MemoryStream())
            {
                _sut.Sync(stream);
                Assert.Equal(Dateizustande.GONE, file.Zustand);
            }
        }

        [Theory]
        [InlineData(Dateizustande.CREATED, Alphabet.CREATE, Dateizustande.CREATED)]
        [InlineData(Dateizustande.CREATED, Alphabet.MODIFY, Dateizustande.CREATED)]
        [InlineData(Dateizustande.CREATED, Alphabet.SYNC, Dateizustande.INSYNC)]
        [InlineData(Dateizustande.CREATED, Alphabet.DELETE, Dateizustande.GONE)]
        [InlineData(Dateizustande.DELETED, Alphabet.CREATE, Dateizustande.MODIFIED)]
        [InlineData(Dateizustande.DELETED, Alphabet.DELETE, Dateizustande.DELETED)]
        [InlineData(Dateizustande.DELETED, Alphabet.MODIFY, Dateizustande.MODIFIED)]
        [InlineData(Dateizustande.DELETED, Alphabet.SYNC, Dateizustande.GONE)]
        [InlineData(Dateizustande.GONE, Alphabet.CREATE, Dateizustande.CREATED)]
        [InlineData(Dateizustande.GONE, Alphabet.DELETE, Dateizustande.GONE)]
        [InlineData(Dateizustande.GONE, Alphabet.MODIFY, Dateizustande.GONE)]
        [InlineData(Dateizustande.GONE, Alphabet.SYNC, Dateizustande.GONE)]
        public void Update_Test(Dateizustande initialState, Alphabet input, Dateizustande resultState)
        {
            var file = GetFile("datei1", initialState);
            _sut.WatchedFiles = GetDictionary(file);

            var updateEvent = new FileEvent(file, input);
            _sut.Update(updateEvent);

            Assert.Equal(resultState, file.Zustand);
        }

        private static WatchedFile GetFile(string fileName, Dateizustande zustand)
        {
            return new WatchedFile(fileName, "", DateTime.Now, zustand);
        }

        private Dictionary<string, WatchedFile> GetDictionary(WatchedFile file)
        {
            return new Dictionary<string, WatchedFile>
            {
                [file.DateiName] = file
            };
        }
    }
}