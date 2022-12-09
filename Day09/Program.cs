using System.Diagnostics;
using Shared;
using Shared.Enums;
using Shared.Extensions;
using Shared.Objects;

Stopwatch sw = new();
sw.Start();
FileReader reader = new("input.txt");
List<string> lines = reader.ReadStringLines();

int sampleSize = 1000;
int p1 = 0;
int p2 = 0;

int[,] matrix = new int[sampleSize, sampleSize];
MatrixPosition tailPosition = new(sampleSize / 2, sampleSize / 2);
MatrixPosition headPosition = new(sampleSize / 2, sampleSize / 2);
List<MatrixPosition> tailHistory = new List<MatrixPosition>();
tailHistory.Add(tailPosition);
foreach (var line in lines)
{
  var lineParts = line.Split(" ");
  MatrixDirection direction = GetDirection(lineParts[0]);
  if (!int.TryParse(lineParts[1], out int numberOfMoves)) throw new Exception("Could not parse");

  // Console.WriteLine(line);
  for (int i = 0; i < numberOfMoves; i++)
  {
    headPosition = MoveHeadToDirection(headPosition, direction);
    // Console.WriteLine($"Head: {headPosition}");
    tailPosition = MoveTailCloserToHeader(tailPosition, headPosition);
    // Console.WriteLine($"Tail: {tailPosition}");
  }

  // Console.WriteLine();
}

p1 = tailHistory.Distinct().Count();

sw.Stop();
Console.WriteLine($"Part one: {p1}");
Console.WriteLine($"Part two: {p2}");
Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");

MatrixPosition MoveTailCloserToHeader(MatrixPosition tailPosition, MatrixPosition headPosition)
{
  if (tailPosition == headPosition) return tailPosition;
  List<MatrixPosition> neighbours = matrix.GetNeighbourPositions(headPosition, true);
  if (neighbours.All(x => x != tailPosition))
  {
    List<MatrixPosition> tailNeighbours = matrix.GetNeighbourPositions(tailPosition, true);
    MatrixPosition? newTailPosition = tailNeighbours.FirstOrDefault(tail => neighbours.Contains(tail));
    if (newTailPosition is null)
    {
      throw new Exception("Could not move tail");
    }

    tailHistory.Add(newTailPosition);
    return newTailPosition;
  }

  return tailPosition;
}

MatrixPosition MoveHeadToDirection(MatrixPosition position, MatrixDirection direction)
{
  return matrix.GetPositionsInDirection(position, direction, MatrixDepth.One).First();
}

MatrixDirection GetDirection(string direction)
{
  if (direction.Equals("u", StringComparison.InvariantCultureIgnoreCase)) return MatrixDirection.Up;
  if (direction.Equals("d", StringComparison.InvariantCultureIgnoreCase)) return MatrixDirection.Down;
  if (direction.Equals("l", StringComparison.InvariantCultureIgnoreCase)) return MatrixDirection.Left;
  if (direction.Equals("r", StringComparison.InvariantCultureIgnoreCase)) return MatrixDirection.Right;
  throw new Exception("Could not parse direction");
}