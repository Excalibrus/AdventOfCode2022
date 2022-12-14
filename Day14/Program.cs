using System.Diagnostics;
using Day14;
using Shared;
using Shared.Extensions;

Stopwatch sw = new();
sw.Start();
FileReader reader = new("input.txt");
List<string> inputLines = reader.ReadStringLines();

bool part1 = false;
int counter = 0;

List<MaterialPoint> caveMap = new();
foreach (string inputLine in inputLines)
{
  List<MaterialPoint> linePoints = inputLine
    .Split(" -> ")
    .Select(x =>
      new MaterialPoint(
        int.Parse(x.Split(",")[0]),
        int.Parse(x.Split(",")[1]),
        MaterialType.Rock
      ))
    .ToList();
  if (linePoints.Count == 1)
  {
    caveMap.Add(linePoints.First());
  }
  else if (linePoints.Count > 1)
  {
    for (int i = 0; i < linePoints.Count - 1; i++)
    {
      List<Point> pointsBetween = linePoints[i].GetPointsBetween(linePoints[i + 1]);
      caveMap.AddRange(pointsBetween.Select(x => new MaterialPoint(x, MaterialType.Rock)));
      caveMap.Add(linePoints[i]);
      if (i == linePoints.Count - 2)
      {
        caveMap.Add(linePoints[i + 1]);
      }
    }
  }
}

caveMap = caveMap.Distinct().ToList();

int maxRockY = caveMap.OrderByDescending(x => x.Y).First().Y + (part1 ? 0 : 2);

Point initSandPoint = new(500, 0);
int p1 = 0;
bool canLand = true;

while (canLand)
{
  List<MaterialPoint>? points = GetSandLanding(new MaterialPoint(initSandPoint, MaterialType.Sand));
  // points.Reverse();
  if (points.First().SandDirection is Direction.DownLeft)
  {
    List<MaterialPoint> leftPoints = points.TakeWhile(x => x.SandDirection == Direction.DownLeft).ToList();
    if (leftPoints.Count > 1)
    {
      leftPoints = leftPoints.Take(leftPoints.Count - 1).ToList();
    }
    points = leftPoints;
  }
  else if (points.First().SandDirection is Direction.DownRight)
  {
    List<MaterialPoint> rightPoints = points.TakeWhile(x => x.SandDirection == Direction.DownRight).ToList();
    if (rightPoints.Count > 1)
    {
      rightPoints = rightPoints.Take(rightPoints.Count - 1).ToList();
    }
    points = rightPoints;
  }
  else
  {
    points = points.Take(1).ToList();
  }
  if (points.Count == 0 || points.Last() == initSandPoint)
  {
    canLand = false;
  }
  else
  {
    caveMap.AddRange(points);
    p1 += points.Count;
    counter++;
    if (counter % 1000 == 0)
    {
      // DrawCave(p1);
      WriteProgress(p1);
    }

    // DrawCave(p1);
  }
}

// DrawCave(0);

sw.Stop();


Console.WriteLine($"Part one: {p1}");
Console.WriteLine($"Part two: {p1 + 1}");
Console.WriteLine($"Time: {sw.Elapsed}");

List<MaterialPoint> GetSandLanding(MaterialPoint sandPoint)
{
  if (sandPoint.Y == maxRockY && part1) return new List<MaterialPoint>();

  DirectionResult down = NextPointInDirection(sandPoint, Direction.Down);
  if (down.Continue)
  {
    List<MaterialPoint> downPoints = GetSandLanding(new MaterialPoint(down.NextPoint, MaterialType.Sand));
    sandPoint.SandDirection = null;
    downPoints.Add(sandPoint);
    return downPoints;
  }
  
  DirectionResult right = NextPointInDirection(sandPoint, Direction.DownRight);
  if (right.Continue)
  {
    List<MaterialPoint> rightPoints =
      GetSandLanding(new MaterialPoint(right.NextPoint, MaterialType.Sand, Direction.DownLeft));
    sandPoint.SandDirection = Direction.DownRight;
    rightPoints.Add(sandPoint);
    return rightPoints;
  }
  
  DirectionResult left = NextPointInDirection(sandPoint, Direction.DownLeft);
  if (left.Continue)
  {
    List<MaterialPoint> leftPoints =
      GetSandLanding(new MaterialPoint(left.NextPoint, MaterialType.Sand, Direction.DownLeft));
    sandPoint.SandDirection = Direction.DownLeft;
    leftPoints.Add(sandPoint);

    return leftPoints;
  }

  return new List<MaterialPoint> { sandPoint };
}

DirectionResult NextPointInDirection(MaterialPoint sandPoint, Direction direction)
{
  Point point = sandPoint.GoInDirection(direction);
  MaterialPoint? downMatPoint =
    part1 || point.Y < maxRockY ? caveMap.FirstOrDefault(x => x == point) : new MaterialPoint(point, MaterialType.Rock);
  return new DirectionResult
  {
    OriginalPoint = sandPoint,
    NextPoint = point,
    NextMaterial = downMatPoint
  };
}

void WriteProgress(int i)
{
  Console.WriteLine(i + ")");
  List<MaterialPoint> orderedX = caveMap.OrderBy(x => x.X).ToList();
  List<MaterialPoint> orderedY = caveMap.OrderBy(x => x.Y).ToList();
  int minY = orderedY.First().Y;
  int maxY = orderedY.Last().Y;
  int minX = orderedX.First().X;
  int maxX = orderedX.Last().X;
  Console.WriteLine($"X range: {minX}-{maxX}");
  Console.WriteLine($"Y range: {minY}-{maxY}");
}

void DrawCave(int i)
{
  Console.WriteLine(i + ")");
  List<MaterialPoint> orderedX = caveMap.OrderBy(x => x.X).ToList();
  List<MaterialPoint> orderedY = caveMap.OrderBy(x => x.Y).ToList();
  int minY = orderedY.First().Y;
  int maxY = orderedY.Last().Y;
  int minX = orderedX.First().X;
  int maxX = orderedX.Last().X;

  for (int y = minY; y <= maxY; y++)
  {
    for (int x = minX; x <= maxX; x++)
    {
      if (x == minX)
      {
        Console.Write(y + " ");
      }

      MaterialPoint? point = caveMap.FirstOrDefault(p => p == new Point(x, y));
      if (point is null)
      {
        Console.Write(".");
      }
      else if (point == caveMap.Last())
      {
        Console.Write("x");
      }
      else
      {
        Console.Write(point.Material == MaterialType.Rock ? "#" : "o");
      }
    }

    Console.WriteLine();
  }
}