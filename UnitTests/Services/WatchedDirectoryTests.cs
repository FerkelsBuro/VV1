using Domain.Models;
using Domain.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.Services
{
    public class WatchedDirectoryTests
    {
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
            var file = new WatchedFile("datei 1", "", DateTime.Now, initialState);

            var sut = new WatchedDirectory
            {
                watchedFiles = new Dictionary<string, WatchedFile>
                {
                    [file.DateiName] = file
                }
            };

            var updateEvent = new FileEvent(file.DateiName, input);
            sut.Update(updateEvent);

            Assert.Equal(resultState, file.Zustand);
        }
    }
}