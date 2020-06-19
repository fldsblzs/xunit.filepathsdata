using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Xunit.Sdk;

namespace Xunit.FilePathsData
{
    /// <summary>
    /// Collects file paths from the specified directory.
    /// </summary>
    public class FilePathsDataAttribute : DataAttribute
    {
        private const string pathNullOrEmpty = "The provided path cannot be null or empty!";
        private const string fileNotFound = "The provided file does not exists!";
        private const string directoryNotFound = "The provided directory does not exists!";

        private readonly string _providedPath;
        private readonly string _providedSearchPattern;

        /// <summary>
        /// Collects file paths from the specified directory. A single file can be specified and will be returned.
        /// </summary>
        /// <param name="path">Absolute or relative path to a file or a directory.</param>
        public FilePathsDataAttribute(string path)
            : this(path, default) { }

        /// <summary>
        /// Collects file paths from the specified directory. A single file can be specified and will be returned.
        /// </summary>
        /// <param name="path">The absolute or relative path to a single file or a directory.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
        public FilePathsDataAttribute(string path, string searchPattern)
        {
            _providedPath = path;
            _providedSearchPattern = searchPattern;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            ValidateArguments(testMethod);

            var fullPath = new FileHelper().GetFullPath(_providedPath);
            var filePathsToReturn = CollectAllFilePaths(fullPath);

            return filePathsToReturn
                .Select(file => new[] { file })
                .AsEnumerable();
        }

        private string[] CollectAllFilePaths(string fullPath)
        {
            if (Path.HasExtension(fullPath))
            {
                if (!File.Exists(fullPath))
                {
                    throw new ArgumentException(fileNotFound);
                }

                return new[] { fullPath };
            }

            if (!Directory.Exists(fullPath))
            {
                throw new ArgumentException(directoryNotFound);
            }

            return string.IsNullOrEmpty(_providedSearchPattern)
                ? Directory.GetFiles(fullPath)
                : Directory.GetFiles(fullPath, _providedSearchPattern);
        }

        private void ValidateArguments(MethodInfo testMethod)
        {
            if (testMethod is null)
            {
                throw new ArgumentNullException(nameof(testMethod));
            }

            if (string.IsNullOrEmpty(_providedPath))
            {
                throw new ArgumentException(pathNullOrEmpty);
            }
        }
    }
}
