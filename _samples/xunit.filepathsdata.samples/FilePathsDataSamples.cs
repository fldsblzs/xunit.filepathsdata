using System.IO;
using System.Text.Json;
using Xunit.FilePathsData;
using Xunit.FilePathsData.Samples.Models;

namespace Xunit.Filepathsdata.Samples
{
    public class FilePathsDataSamples
    {
        /// <summary>
        /// Sample for specifying a folder.
        /// </summary>
        [Theory]
        [FilePathsData("TestFiles")]
        public void FolderSamle(string filePath)
        {
            // Arrange
            var jsonString = File.ReadAllText(filePath);
            var book = JsonSerializer.Deserialize<Book>(jsonString);

            // Act
            // ...

            // Assert
            Assert.True(true);
        }

        /// <summary>
        /// Sample for specifying a folder and a search pattern.
        /// </summary>
        [Theory]
        [FilePathsData("TestFiles", "testFile?.json")]
        public void FolderAndSearchPatternSample(string filePath)
        {
            // Arrange
            var jsonString = File.ReadAllText(filePath);
            var book = JsonSerializer.Deserialize<Book>(jsonString);

            // Act
            // ...

            // Assert
            Assert.True(true);
        }

        /// <summary>
        /// Sample for specifying a folder and a search pattern 2.
        /// </summary>
        [Theory]
        [FilePathsData("..\\..\\..\\..\\..\\test\\xunit.filepathsdata.tests\\TestFiles", "*.json")]
        public void FolderAndSearchPatternSample2(string filePath)
        {
            // Arrange
            var jsonString = File.ReadAllText(filePath);
            var book = JsonSerializer.Deserialize<Book>(jsonString);

            // Act
            // ...

            // Assert
            Assert.True(true);
        }

        /// <summary>
        /// Sample for specifying a single file.
        /// </summary>
        [Theory]
        [FilePathsData("TestFiles\\testFile1.json")]
        public void SingleFileSample(string filePath)
        {
            // Arrange
            var jsonString = File.ReadAllText(filePath);
            var book = JsonSerializer.Deserialize<Book>(jsonString);

            // Act
            // ...

            // Assert
            Assert.True(true);
        }
    }
}
