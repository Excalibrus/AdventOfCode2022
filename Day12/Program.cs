using System.Diagnostics;
using Shared;
using Shared.Extensions;
using Shared.Objects;

Stopwatch sw = new();
sw.Start();
FileReader reader = new("input_demo.txt");

List<string> lines = reader.ReadStringLines();

MatrixPosition startPosition = new(0, 0);
MatrixPosition endPosition = new(0, 0);

char[,] matrix = new char[lines.Count, lines[0].Length];
int[,] countMatrix = new int[lines.Count, lines[0].Length];
for (int row = 0; row < lines.Count; row++)
{
  for (int col = 0; col < lines[row].Length; col++)
  {
    countMatrix[row, col] = int.MaxValue;
    char letter = lines[row][col];
    if (letter == 'S')
    {
      startPosition = new MatrixPosition(row, col);
      matrix.SetValue('a', row, col);
    }
    else if (letter == 'E')
    {
      endPosition = new MatrixPosition(row, col);
      matrix.SetValue('z', row, col);
    }
    else
    {
      matrix.SetValue(letter, row, col);
    }
  }
}

int minPath = 500; // int.max for fuller version
GoToNextPosition(matrix, new List<MatrixPosition> { startPosition });

sw.Stop();
Console.WriteLine($"Part one: {minPath}");
Console.WriteLine($"Part two: {0}");
Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");

void GoToNextPosition(char[,] matrix, List<MatrixPosition> path)
{
  if (path.Count > minPath) return;
  MatrixPosition fromPosition = path.Last();
  if (path.Count < countMatrix[fromPosition.Row, fromPosition.Col])
  {
    countMatrix[fromPosition.Row, fromPosition.Col] = path.Count;
  }
  else
  {
    return;
  }
  // Console.WriteLine($"Depth {path.Count}, Min: {minPath}, Elevation: {matrix[fromPosition.Row, fromPosition.Col]}, Size: {countMatrix[fromPosition.Row, fromPosition.Col]}");
  // if (countMatrix[fromPosition.Row, fromPosition.Col] > path.Count) return;
  List<MatrixPosition> neighbourPositions = matrix.GetNeighbourPositions(fromPosition, MatrixDirection.Cross);
  List<MatrixPosition> possibleNextPositions = neighbourPositions.Where(x =>
    !path.Contains(x) &&
    matrix[fromPosition.Row, fromPosition.Col] + 1 >= matrix[x.Row, x.Col]
  ).ToList();
  if (possibleNextPositions.Contains(endPosition))
  {
    if (path.Count <= minPath)
    {
      minPath = path.Count;
      Console.WriteLine($"Found path {minPath}");
      //   for (int row = 0; row < matrix.GetLength(0); row++)
      //   {
      //     for (int col = 0; col < matrix.GetLength(1); col++)
      //     {
      //       Console.Write(path.Contains(new MatrixPosition(row, col)) ? "X" : matrix[row, col]);
      //     }
      //     Console.WriteLine();
      //   }
    }

    return;
  }

  foreach (MatrixPosition nextPosition in possibleNextPositions)
  {
    GoToNextPosition(matrix, new List<MatrixPosition>(path) { nextPosition });
  }
}