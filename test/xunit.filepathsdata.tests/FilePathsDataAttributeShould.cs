using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Moq;
using Xunit.FilePathsData;

namespace Xunit.Filepathsdata.Tests
{
    public class FilePathsDataAttributeShould
    {
        private readonly Mock<MethodInfo> _methodInfo;

        public FilePathsDataAttributeShould() => _methodInfo = new Mock<MethodInfo>();

        [Theory]
        [InlineData(null, "The provided path cannot be null or empty!")]
        [InlineData("", "The provided path cannot be null or empty!")]
        [InlineData("\\relative\\folder\\does\not\\exists", "The provided directory does not exists!")]
        [InlineData("\\relative\\folder\\does\not\\exists\\file.txt", "The provided file does not exists!")]
        [InlineData("C:\\relative\\folder\\does\not\\exists", "The provided directory does not exists!")]
        [InlineData("C:\\relative\\folder\\does\not\\exists\\file.txt", "The provided file does not exists!")]
        [InlineData("file.txt", "The provided file does not exists!")]
        public void FileDataAttributeShould_ThrowForInvalidPaths(string path, string expectedMessage)
        {
            // Arrange
            var sut = new FilePathsDataAttribute(path);

            // Act
            var exception = Assert.Throws<ArgumentException>(() => sut.GetData(_methodInfo.Object));

            // Assert
            exception.Should().BeOfType<ArgumentException>();
            exception.Message.Should().Be(expectedMessage);
        }

        [Theory]
        [InlineData("TestFiles\\testFile1.json")]
        [InlineData("TestFiles\\testFile1.txt")]
        [InlineData("TestFiles\\testFile1.xml")]
        public void FileDataAttributeShould_ReturnSingleFileSpecified(string path)
        {
            // Arrange
            var sut = new FilePathsDataAttribute(path);

            var expected = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

            // Act
            var results = sut.GetData(_methodInfo.Object);

            // Assert
            results.Should().HaveCount(1);
            results.First().First().ToString().Should().Be(expected);
        }

        [Fact]
        public void FileDataAttributeShould_ReturnAllFilesFromFolderSpecified()
        {
            // Arrange
            var folder = "TestFiles";
            var sut = new FilePathsDataAttribute(folder);

            // Act
            var results = sut.GetData(_methodInfo.Object);

            // Assert
            results.Should().HaveCount(9);
        }

        [Theory]
        [InlineData("none.*", 0)]
        [InlineData("*.*", 9)]
        [InlineData("*File*.*", 9)]
        [InlineData("testFile*.*", 9)]
        [InlineData("testFile?.*", 9)]
        [InlineData("testFile1.*", 3)]
        [InlineData("testFile?.json", 3)]
        [InlineData("*.xml", 3)]
        [InlineData("testFile1.txt", 1)]
        public void FileDataAttributeShould_ReturnAllFilesFromFolderSpecifiedWithSearchPattern(string searchPattern, int numberOfExpectedFiles)
        {
            // Arrange
            var folder = "TestFiles";
            var sut = new FilePathsDataAttribute(folder, searchPattern);

            // Act
            var results = sut.GetData(_methodInfo.Object);

            // Assert
            results.Should().HaveCount(numberOfExpectedFiles);
        }
    }
}
