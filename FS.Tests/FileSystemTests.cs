using System.IO;
using NUnit.Framework;

namespace FS.Tests
{
    [TestFixture]
    public class FileSystemTests
    {
        [SetUp]
        public void SetUp() {
            DeleteDirectory("a");
            DeleteDirectory("b");
        }

        private static void DeleteDirectory(string path) {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        [Test]
        public void CanCopyDirectory() {
            CreateFiles(@"a\one.txt", @"a\inner\two.txt");

            new FileSystem().Copy("a", "b");

            AssertFileContents(@"b\one.txt", @"contents of a\one.txt");
            AssertFileContents(@"b\inner\two.txt", @"contents of a\inner\two.txt");
        }

        [Test]
        public void CanMoveDirectory() {
            CreateFiles(@"a\one.txt", @"a\inner\two.txt");

            new FileSystem().Move("a", "b");

            AssertDirectoryNotExtant(@"a");
            AssertFileContents(@"b\one.txt", @"contents of a\one.txt");
            AssertFileContents(@"b\inner\two.txt", @"contents of a\inner\two.txt");
        }

        [Test]
        public void CanCopyDirectoryExcludingSomeFiles() {
            CreateFiles(@"a\one.bad", @"a\inner\two.txt");

            new FileSystem().Copy("a", "b", fn => Path.GetExtension(fn) != ".bad");

            AssertFileNotExtant(@"b\one.bad");
            AssertFileContents(@"b\inner\two.txt", @"contents of a\inner\two.txt");
        }

        [Test]
        public void CanCopyFile() {
            CreateFiles(@"a\one.txt");

            new FileSystem().Copy(@"a\one.txt", @"b\one.txt");

            AssertFileContents(@"b\one.txt", @"contents of a\one.txt");
        }

        [Test]
        public void CanMoveFile() {
            CreateFiles(@"a\one.txt");
            CreateDirectoryPath("b");

            new FileSystem().Move(@"a\one.txt", @"b\one.txt");

            AssertFileNotExtant(@"a\one.txt");
            AssertFileContents(@"b\one.txt", @"contents of a\one.txt");
        }

        [Test]
        public void ItMovesFilesAndCreatesDestinationDirectoriesIfNecessary() {
            CreateFiles(@"a\one.txt");

            new FileSystem().Move(@"a\one.txt", @"b\one.txt");

            AssertFileNotExtant(@"a\one.txt");
            AssertFileContents(@"b\one.txt", @"contents of a\one.txt");
        }

        [Test]
        public void CanCopyAFileToDirectory() {
            CreateFiles(@"a\one.txt");

            new FileSystem().CopyToDirectory(@"a\one.txt", "b");
            AssertFileContents(@"b\one.txt", @"contents of a\one.txt");
        }

        [Test]
        public void CanMoveAFileToDirectory() {
            CreateFiles(@"a\one.txt");

            new FileSystem().MoveToDirectory(@"a\one.txt", "b");
            AssertFileNotExtant(@"a\one.txt");
            AssertFileContents(@"b\one.txt", @"contents of a\one.txt");
        }

        [Test]
        public void CanCopyADirectoryToDirectory()
        {
            CreateFiles(@"a\one.txt", @"a\inner\two.txt");

            new FileSystem().CopyToDirectory("a", "b");

            AssertFileContents(@"b\a\one.txt", @"contents of a\one.txt");
            AssertFileContents(@"b\a\inner\two.txt", @"contents of a\inner\two.txt");
        }

        [Test]
        public void CanMoveADirectoryToDirectory()
        {
            CreateFiles(@"a\one.txt", @"a\inner\two.txt");

            new FileSystem().MoveToDirectory("a", "b");

            AssertDirectoryNotExtant(@"a");
            AssertFileContents(@"b\a\one.txt", @"contents of a\one.txt");
            AssertFileContents(@"b\a\inner\two.txt", @"contents of a\inner\two.txt");
        }

        [Test]
        public void CanDeleteDirectory() {
            CreateFiles(@"a\one.txt", @"a\inner\two.txt");

            new FileSystem().Delete("a");

            AssertDirectoryNotExtant("a");
        }

        [Test]
        public void CanDeleteFile() {
            CreateFiles(@"a\one.txt");

            new FileSystem().Delete(@"a\one.txt");

            AssertFileNotExtant(@"a\one.txt");
        }

        [Test]
        public void DoesntGoCrazyWhenFileDoenstExistForDeleting() {
            new FileSystem().Delete(@"a\one.txt");

            AssertFileNotExtant(@"a\one.txt");
        }

        private static void AssertFileNotExtant(string path) {
            Assert.False(File.Exists(path), "expected {0} not to exist", path);
        }

        private static void AssertDirectoryNotExtant(string path) {
            Assert.False(Directory.Exists(path), "expected {0} not to exist", path);
        }

        private void CreateFiles(params string[] paths) {
            foreach (var path in paths) {
                new FileSystem().CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path, "contents of " + path);
            }
        }

        private void CreateDirectoryPath(string path) {
            if (path != string.Empty && !Directory.Exists(path)) {
                if (File.Exists(path)) {
                    File.Delete(path);
                }
                CreateDirectoryPath(Path.GetDirectoryName(path));
                Directory.CreateDirectory(path);
            }
        }

        private void AssertFileContents(string path, string contents) {
            Assert.True(File.Exists(path), "expected {0} to exist", path);
            Assert.That(File.ReadAllText(path), Is.EqualTo(contents));
        }
    }
}
