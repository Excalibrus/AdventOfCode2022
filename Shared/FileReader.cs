namespace Shared;

public class FileReader
{
  private readonly FileInfo _fileInfo;

  public FileReader(string fileName = "input.txt")
  {
    _fileInfo = new FileInfo(fileName);
    if (!_fileInfo.Exists)
    {
      throw new Exception("File does not exist");
    }
  }

  public List<string> ReadStringLines()
  {
    return File.ReadAllLines(_fileInfo.FullName).ToList();
  }

  public List<int> ReadIntLines()
  {
    return ReadStringLines().Select(int.Parse).ToList();
  }

  public int[,] ReadIntMatrix()
  {
    List<string> lines = ReadStringLines();
    int[,] matrix = new int[lines.Count, lines[0].Length];

    for (int x = 0; x < lines.Count; x++)
    {
      for (int y = 0; y < lines[x].Length; y++)
      {
        matrix[x, y] = int.Parse(lines[x][y].ToString());
      }
    }
    return matrix;
  }
}