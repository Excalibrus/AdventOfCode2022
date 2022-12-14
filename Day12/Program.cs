using System.Diagnostics;
using Shared;
using Shared.Extensions;
using Shared.Objects;

Stopwatch sw = new();
sw.Start();
FileReader reader = new("input.txt");

List<string> lines = reader.ReadStringLines();

MatrixPosition startPosition = new(0, 0);
MatrixPosition endPosition = new(0, 0);

List<MatrixPosition> otherStartingPositions = new();

char[,] matrix = new char[lines.Count, lines[0].Length];
int[,] countMatrixInit = new int[lines.Count, lines[0].Length];
for (int row = 0; row < lines.Count; row++)
{
  for (int col = 0; col < lines[row].Length; col++)
  {
    countMatrixInit[row, col] = int.MaxValue;
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
      if (letter == 'a')
      {
        otherStartingPositions.Add(new MatrixPosition(row, col));
      }
      matrix.SetValue(letter, row, col);
    }
  }
}

int minPath = 500; // int.max for fuller version
GoToNextPosition(matrix, new List<MatrixPosition> { startPosition }, (int[,])countMatrixInit.Clone());

Console.WriteLine($"Part one: {minPath}");

decimal processorUsage = 0.8m;
int degree = Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * processorUsage) * (decimal)2.0));
int processed = 0;
Parallel.For(0, otherStartingPositions.Count, i =>
{
  new ParallelOptions
  {
    MaxDegreeOfParallelism = degree
  };
  MatrixPosition startingPos = otherStartingPositions[i];
  processed++;
  decimal percent = ((decimal)processed / (decimal)otherStartingPositions.Count) * 100;
  Console.WriteLine($"{processed}/{otherStartingPositions.Count} ({Math.Round(percent, 2)}%))Starting position {startingPos}");
  GoToNextPosition(matrix, new List<MatrixPosition> { startingPos }, (int[,])countMatrixInit.Clone());
});


sw.Stop();

Console.WriteLine($"Part two: {minPath}");
Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");

void GoToNextPosition(char[,] matrix, List<MatrixPosition> path, int[,] countMatrix)
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
  List<MatrixPosition> neighbourPositions = matrix.GetNeighbourPositions(fromPosition, Direction.Cross);
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
    GoToNextPosition(matrix, new List<MatrixPosition>(path) { nextPosition }, countMatrix);
  }
}