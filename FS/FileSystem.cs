using System;
using System.IO;

namespace FS
{
    public class FileSystem
    {
        public void Copy(string from, string to, Predicate<string> copyFile = null) {
            if (Directory.Exists(from)) {
                CopyDirectoryContents(from, from, to, copyFile ?? (fn => true));
            } else {
                CopyFile(from, to);
            }
        }

        public void CopyToDirectory(string from, string toDirectory, Predicate<string> copyFile = null)
        {
            Copy(from, Path.Combine(toDirectory, Path.GetFileName(from)), copyFile);
        }

        public void Delete(string path) {
            if (Directory.Exists(path)) {
                Directory.Delete(path, true);
            } else if (File.Exists(path)) {
                File.Delete(path);
            }
        }

        public void Move(string from, string to) {
            CreateDirectory(Path.GetDirectoryName(to));
            if (Directory.Exists(from)) {
                Directory.Move(from, to);
            } else if (File.Exists(from)) {
                File.Move(from, to);
            }
        }

        public void MoveToDirectory(string from, string toDirectory) {
            Move(from, Path.Combine(toDirectory, Path.GetFileName(from)));
        }

        public void CreateDirectory(string path) {
            if (path != string.Empty && !Directory.Exists(path)) {
                if (File.Exists(path)) {
                    File.Delete(path);
                }
                CreateDirectory(Path.GetDirectoryName(path));
                Directory.CreateDirectory(path);
            }
        }

        private void CopyFile(string from, string to) {
            var toDirectory = Path.GetDirectoryName(to);
            if (toDirectory != String.Empty && !Directory.Exists(toDirectory))
            {
                Directory.CreateDirectory(toDirectory);
            }

            File.Copy(@from, to);
        }

        private static void CopyDirectoryContents(string originalFrom, string from, string to, Predicate<string> copyFile)
        {
            if (!Directory.Exists(to))
            {
                Directory.CreateDirectory(to);
            }

            foreach (var file in Directory.GetFiles(@from))
            {
                var destFilename = Path.Combine(to, Path.GetFileName(file));

                if (copyFile(file.Substring(originalFrom.Length + 1)))
                {
                    File.Copy(file, destFilename);
                }
            }

            foreach (var subdir in Directory.GetDirectories(@from))
            {
                if (copyFile(subdir.Substring(originalFrom.Length + 1) + @"\"))
                {
                    CopyDirectoryContents(originalFrom, subdir, Path.Combine(to, Path.GetFileName(subdir)),
                        copyFile);
                }
            }
        }
    }
}
