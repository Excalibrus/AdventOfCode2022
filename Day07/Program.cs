using System.Diagnostics;
using Shared;

Stopwatch sw = new();
sw.Start();
FileReader reader = new("input.txt");
List<string> lines = reader.ReadStringLines();

File currentFile = new()
{
  Name = "root",
  Size = 0,
  IsDirectory = true
};

long availableSpace = 70_000_000;
long requiredSpace = 30_000_000;

Command currentCommand = Command.ChangeDirectory;
foreach (string line in lines)
{
  // commans
  if (line.StartsWith("$"))
  {
    currentCommand = line.Contains("$ cd") ? Command.ChangeDirectory : Command.ListDirectory;
    if (currentCommand == Command.ChangeDirectory)
    {
      string dirName = line.Split(" ").Last();
      if (dirName.Equals(".."))
      {
        if (currentFile.ParentReference is not null)
        {
          currentFile = currentFile.ParentReference;
        }
      }
      else if (dirName.Equals("/"))
      {
        currentFile = currentFile.GetRootDir();
      }
      else
      {
        if (!currentFile.Files.Any(x => x.Name.Equals(dirName)))
        {
          currentFile.Files.Add(new File
          {
            Name = dirName,
            IsDirectory = true,
            ParentReference = currentFile
          });
        }

        currentFile = currentFile.Files.FirstOrDefault(x => x.IsDirectory && x.Name.Equals(dirName)) ??
                      throw new Exception("This directory does not exist");
      }
    }
  }
  else if (currentCommand == Command.ListDirectory)
  {
    string fileP1 = line.Split(" ").First();
    string fileP2 = line.Split(" ").Last();
    if (fileP1.Equals("dir"))
    {
      currentFile.Files.Add(new File
      {
        Name = fileP2,
        IsDirectory = true,
        ParentReference = currentFile
      });
    }
    else
    {
      currentFile.Files.Add(new File
      {
        Name = fileP2,
        ParentReference = currentFile,
        Size = int.Parse(fileP1)
      });
    }
  }
}

// go to root

currentFile = currentFile.GetRootDir();

long p1 = currentFile.GetDirectorySizesBelowThreshold(100_000).Sum(x => x);

long freeSpace = availableSpace - currentFile.TotalSize;
long thresholdP2 = requiredSpace - freeSpace;
long p2 = currentFile.GetDirectorySizesAboveThreshold(thresholdP2).OrderBy(x => x).First();

sw.Stop();
Console.WriteLine($"Part one: {p1}");
Console.WriteLine($"Part two: {p2}");
Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");


static class FileHelpers
{
  internal static List<long> GetDirectorySizesAboveThreshold(this File file, long threshold)
  {
    if (!file.IsDirectory) return new List<long>();
    
    List<long> sizes = file.Files.SelectMany(x => x.GetDirectorySizesAboveThreshold(threshold)).ToList();
    if (file.TotalSize >= threshold)
    {
      sizes.Add(file.TotalSize);
    }

    return sizes;
  }
  
  internal static List<long> GetDirectorySizesBelowThreshold(this File file, long threshold)
  {
    if (!file.IsDirectory) return new List<long>();
    
    List<long> sizes = file.Files.SelectMany(x => x.GetDirectorySizesBelowThreshold(threshold)).ToList();
    if (file.TotalSize <= threshold)
    {
      sizes.Add(file.TotalSize);
    }

    return sizes;
  }

  internal static File GetRootDir(this File file)
  {
    while (file.ParentReference is not null)
    {
      file = file.ParentReference;
    }

    return file;
  }
}

class File {
  public string Name { get; set; }
  public bool IsDirectory { get; set; }
  public long Size { get; set; }
  public File? ParentReference { get; set; }
  public List<File> Files { get; set; } = new();
  public long TotalSize => Size + Files.Sum(x => x.TotalSize);
}

enum Command
{
  ChangeDirectory = 1,
  ListDirectory = 2
}