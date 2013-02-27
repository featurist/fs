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

## Delete

    fs.Delete("file.txt");
    fs.Delete("directory");

## Create

Creates the full path:

    fs.CreateDirectory(@"a\long\path\to\a\directory");

## Why?

Because this is the last time I'll write code to recursively copy a directory in C#.
