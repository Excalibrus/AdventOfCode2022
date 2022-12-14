using System.Diagnostics;
using Day14;
using Shared;
using Shared.Extensions;

Stopwatch sw = new();
sw.Start();
FileReader reader = new("input_demo.txt");
List<string> inputLines = reader.ReadStringLines();



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

int maxRockY = caveMap.OrderByDescending(x => x.Y).First().Y;

Point initSandPoint = new(500, 0);
int p1 = 0;
bool canLand = true;
while (canLand)
{
  MaterialPoint? point = GetSandLanding(new MaterialPoint(initSandPoint, MaterialType.Sand));
  if (point is null || point == initSandPoint)
  {
    canLand = false;
  }
  else
  {
    caveMap.Add(point);
    p1++;
    // DrawCave(p1);
  }
}

// DrawCave(0);

sw.Stop();


Console.WriteLine($"Part one: {p1}");
Console.WriteLine($"Part two: {0}");
Console.WriteLine($"Time: {sw.Elapsed}");

MaterialPoint? GetSandLanding(MaterialPoint sandPoint)
{
  if (sandPoint.Y == maxRockY) return null;

  DirectionResult down = NextPointInDirection(sandPoint, Direction.Down);
  if (down.Continue)
  {
    return GetSandLanding(new MaterialPoint(down.NextPoint, MaterialType.Sand));
  }
  
  DirectionResult left = NextPointInDirection(sandPoint, Direction.DownLeft);
  if (left.Continue)
  {
    return GetSandLanding(new MaterialPoint(left.NextPoint, MaterialType.Sand));
  }
  
  DirectionResult right = NextPointInDirection(sandPoint, Direction.DownRight);
  if (right.Continue)
  {
    return GetSandLanding(new MaterialPoint(right.NextPoint, MaterialType.Sand));
  }

  return sandPoint;
}

DirectionResult NextPointInDirection(MaterialPoint sandPoint, Direction direction)
{
  Point point = sandPoint.GoInDirection(direction);
  MaterialPoint? downMatPoint = caveMap.FirstOrDefault(x => x == point);
  return new DirectionResult
  {
    OriginalPoint = sandPoint,
    NextPoint = point,
    NextMaterial = downMatPoint
  };
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