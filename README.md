# FS

    PM> Install-Package FS

Then

    var fs = new FileSystem();

## Copy

    fs.Copy("fromDirectory", "toDirectory");
    fs.Copy("fromFile.txt", "toFile.txt");

It will even create the missing directories:

    fs.Copy("fromFile.txt", @"path\to\toFile.txt");

Or to copy into a directory:

    fs.CopyToDirectory("fromFile.txt", @"aDirectory");

Don't copy some files:

    fs.Copy("source", "dest", fn => fn != ".git");
  
## Move

    fs.Move("fromDirectory", "toDirectory");
    fs.Move("fromFile.txt", "toFile.txt");

It will even create the missing directories:

    fs.Move("fromFile.txt", @"path\to\toFile.txt");

Or to move into a directory:

    fs.MoveToDirectory("fromFile.txt", @"aDirectory");

## Find

    var paths = fs.Find("directory");

Will return the relative paths of all files and directories inside `directory`, including `directory`. E.g.

    directory
    directory\somefile.txt
    direcotry\subdir
    direcotry\subdir\anotherfile.txt

Find returns a lazy enumeration, so it only finds stuff as you consume the enumeration. For example, if you
were just looking for the first text file on your `C:\` drive:

    var textFile = fs.Find(@"C:\").FirstOrDefault(path => path.EndsWith(".txt"));

It won't go searching the rest of your `C:\` drive.

Find also takes a predicate argument so you can tell it not to recurse into directories that you aren't interested in:

    var paths = fs.Find("directory", dir => dir != ".git");

## Delete

    fs.Delete("file.txt");
    fs.Delete("directory");

## Create

Creates the full path:

    fs.CreateDirectory(@"a\long\path\to\a\directory");

## Why?

Because this is the last time I'll write code to recursively copy a directory in C#.
