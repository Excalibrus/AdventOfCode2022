using System.Diagnostics;
using Shared;
using Shared.Extensions;
using Shared.Objects;

Stopwatch sw = new();
sw.Start();
FileReader reader = new("input.txt");
int[,] matrix = reader.ReadIntMatrix();

int p1 = 0;
int p2 = 0;
for (int i = 0; i < matrix.GetLength(0); i++)
{
  for (int j = 0; j < matrix.GetLength(1); j++)
  {
    // counting outer
    if (i == 0 || j == 0 || i == matrix.GetLength(0) - 1 || j == matrix.GetLength(1) - 1)
    {
      p1++;
    }
    // counting inner
    else
    {
      MatrixPosition position = new MatrixPosition(i, j);
      if (IsTreeVisible(position, matrix[i, j]))
      {
        p1++;
      }

      int height = MultiplyVisibleTreesInAllDirections(position, matrix[position.Row, position.Col]);
      if (height > p2)
      {
        p2 = height;
      }
    }
  }
}

sw.Stop();
Console.WriteLine($"Part one: {p1}");
Console.WriteLine($"Part two: {p2}");
Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");

bool IsTreeVisible(MatrixPosition position, int treeHeight)
{
  bool up = IsTreeVisibleInDirection(position, treeHeight, Direction.Up);
  bool right = IsTreeVisibleInDirection(position, treeHeight, Direction.Right);
  bool down = IsTreeVisibleInDirection(position, treeHeight, Direction.Down);
  bool left = IsTreeVisibleInDirection(position, treeHeight, Direction.Left);

  // Console.WriteLine($"H:{treeHeight}, x:{position.Row}, y:{position.Col}, up: {up}, right:{right}, down:{down}, left:{left}");
  return up || right || down || left;
}

bool IsTreeVisibleInDirection(MatrixPosition position, int treeHeight, Direction direction)
{
  return matrix
    .GetPositionsInDirection(position, direction)
    .Select(x => matrix[x.Row, x.Col])
    .All(x => x < treeHeight);
}

int CountVisibleTreesInDirection(MatrixPosition position, int treeHeight, Direction direction)
{
  var takeWhile = matrix
    .GetPositionsInDirection(position, direction)
    .Select(x => new {height = matrix[x.Row, x.Col], onEdge = matrix.IsPositionOnEdge(new MatrixPosition(x.Row, x.Col))})
    .TakeWhile(x => x.onEdge || x.height < treeHeight)
    .ToList();
  return takeWhile.Any(x => x.onEdge) ? takeWhile.Count : takeWhile.Count + 1;
}

int MultiplyVisibleTreesInAllDirections(MatrixPosition position, int treeHeight)
{
  return CountVisibleTreesInDirection(position, treeHeight, Direction.Up) *
         CountVisibleTreesInDirection(position, treeHeight, Direction.Down) *
         CountVisibleTreesInDirection(position, treeHeight, Direction.Right) *
         CountVisibleTreesInDirection(position, treeHeight, Direction.Left);
}