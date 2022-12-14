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

List<Rope> ropes = new List<Rope>
{
  new Rope(2, sampleSize),
  new Rope(10, sampleSize)
};

int[,] matrix = new int[sampleSize, sampleSize];


foreach (var line in lines)
{
  var lineParts = line.Split(" ");
  Direction direction = GetDirection(lineParts[0]);
  if (!int.TryParse(lineParts[1], out int numberOfMoves)) throw new Exception("Could not parse");

  // Console.WriteLine($"Dir: {lineParts[0]}, Steps: {numberOfMoves}");
  for (int i = 0; i < numberOfMoves; i++)
  {
    foreach (var rope in ropes)
    {
      rope.Knots[0] = MoveHeadToDirection(rope.Knots[0], direction);
      for (int tailIndex = 1; tailIndex < rope.Knots.Count; tailIndex++)
      {
        rope.Knots[tailIndex] = MoveTailCloserToHeader(
          rope.Knots[tailIndex],
          rope.Knots[tailIndex - 1],
          rope.TailHistory,
          tailIndex == rope.Knots.Count - 1);
      }
    }
  }
}

int p1 = ropes[0].TailHistory.Distinct(new MatrixPositionComparer()).Count();
int p2 = ropes[1].TailHistory.Distinct(new MatrixPositionComparer()).Count();

sw.Stop();
Console.WriteLine($"Part one: {p1}");
Console.WriteLine($"Part two: {p2}");
Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");

MatrixPosition MoveTailCloserToHeader(
  MatrixPosition tailPosition,
  MatrixPosition headPosition,
  List<MatrixPosition> ropeTailHistory,
  bool savePosition = true)
{
  if (tailPosition == headPosition) return tailPosition;
  List<MatrixPosition> headNeighbours = matrix.GetNeighbourPositions(headPosition, Direction.All);
  if (headNeighbours.All(x => x != tailPosition))
  {
    MatrixPosition? newTailPosition;
    bool moveCross = headPosition.Row == tailPosition.Row || headPosition.Col == tailPosition.Col;
    if (moveCross)
    {
      List<MatrixPosition> tailNeighbours = matrix.GetNeighbourPositions(tailPosition, Direction.Cross);
      newTailPosition = tailNeighbours.FirstOrDefault(tail => headNeighbours.Contains(tail));
    }
    else
    {
      List<MatrixPosition> tailNeighbours = matrix.GetNeighbourPositions(tailPosition, Direction.Diagonal);
      newTailPosition = tailNeighbours.FirstOrDefault(tail => headNeighbours.Contains(tail));
    }

    if (newTailPosition is null)
    {
      throw new Exception("Could not move tail");
    }

    if (savePosition)
    {
      ropeTailHistory.Add(newTailPosition);
    }

    return newTailPosition;
  }

  return tailPosition;
}

MatrixPosition MoveHeadToDirection(MatrixPosition position, Direction direction)
{
  return matrix.GetPositionsInDirection(position, direction, MatrixDepth.One).First();
}

Direction GetDirection(string direction)
{
  if (direction.Equals("u", StringComparison.InvariantCultureIgnoreCase)) return Direction.Up;
  if (direction.Equals("d", StringComparison.InvariantCultureIgnoreCase)) return Direction.Down;
  if (direction.Equals("l", StringComparison.InvariantCultureIgnoreCase)) return Direction.Left;
  if (direction.Equals("r", StringComparison.InvariantCultureIgnoreCase)) return Direction.Right;
  // if (direction.Equals("u", StringComparison.InvariantCultureIgnoreCase)) return MatrixDirection.Right;
  // if (direction.Equals("d", StringComparison.InvariantCultureIgnoreCase)) return MatrixDirection.Left;
  // if (direction.Equals("l", StringComparison.InvariantCultureIgnoreCase)) return MatrixDirection.Up;
  // if (direction.Equals("r", StringComparison.InvariantCultureIgnoreCase)) return MatrixDirection.Down;
  throw new Exception("Could not parse direction");
}

class Rope
{
  public List<MatrixPosition> Knots { get; set; } = new();
  public List<MatrixPosition> TailHistory { get; set; } = new();

  public Rope(int knots, int sampleSize)
  {
    Knots = Enumerable.Range(1, knots)
      .Select(x => new MatrixPosition(sampleSize / 2, sampleSize / 2))
      .ToList();
    TailHistory.Add(Knots[0]);
  }
}