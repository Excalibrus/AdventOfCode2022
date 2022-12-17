using System.Diagnostics;
using Day17;
using Shared;

Stopwatch sw = new();
sw.Start();
FileReader reader = new("input_demo.txt");

List<Direction> directions = reader.ReadStringLines().ParseDirections();
List<Shape> shapes = InitShapes();

int currentShapeIndex = 0;
int currentDirectionIndex = 0;
// long iterations = 2022;
// long iterations = 1_000_000_000_000;
long iterations = 10_000;

List<Point> playGround = new();

WallConfig config = new()
{
  LeftWallX = -1,
  RightWallX = 7,
  BottomWallY = 3000,
  InitialWinds = 4
};

for (long i = 1; i <= iterations; i++)
{
  if (i % 1000 == 0)
  {
    Console.WriteLine(i);
  }

  // DrawPlayground();
  Shape currentShape = playGround.PrepareInitialShapePosition(shapes[currentShapeIndex], config, 0);
  bool isLanded = false;
  bool initialWind = true;
  while (!isLanded)
  {
    for (int skipWind = 0; skipWind < config.InitialWinds && initialWind; skipWind++)
    {
      // wind
      Direction initDirection = directions[currentDirectionIndex];
      bool canMoveWithWind = initDirection == Direction.Left
        ? currentShape.MaxLeftPositionX != currentShape.InitialXPosition
        : currentShape.MaxRightPositionX != currentShape.InitialXPosition;

      if (canMoveWithWind)
      {
        int moveX = initDirection == Direction.Left ? -1 : 1;
        currentShape.InitialXPosition += moveX;
        currentShape.MoveX(moveX);
      }

      if (skipWind != config.InitialWinds - 1)
      {
        if (currentDirectionIndex == directions.Count - 1)
        {
          currentDirectionIndex = 0;
        }
        else
        {
          currentDirectionIndex++;
        }
      }
    }

    if (!initialWind)
    {
      // wind
      Direction direction = directions[currentDirectionIndex];
      bool preventMoving =
        currentShape.WindPoints(direction).Any(windPoint =>
          playGround.Where(x => x.Y == windPoint.Y).Contains(
            new Point(windPoint.X + (direction == Direction.Left ? -1 : 1), windPoint.Y)
          ) ||
          windPoint.X == (direction == Direction.Left ? config.LeftWallX + 1 : config.RightWallX - 1)
        );
      if (!preventMoving)
      {
        currentShape.MoveX(direction == Direction.Left ? -1 : 1);
      }
    }

    initialWind = false;


    // try to land
    bool canLand =
      currentShape.LandingPoints().Any(landingPoint =>
        playGround.Where(x => x.X == landingPoint.X)
          .Contains(new Point(landingPoint.X, landingPoint.Y + 1)) ||
        config.BottomWallY - 1 == landingPoint.Y
      );

    if (canLand)
    {
      // landed
      playGround.AddRange(currentShape.Points);
      isLanded = true;
      if (currentShapeIndex == shapes.Count - 1)
      {
        currentShapeIndex = 0;
      }
      else
      {
        currentShapeIndex++;
      }
    }
    else
    {
      currentShape.MoveY(1);
    }

    if (currentDirectionIndex == directions.Count - 1)
    {
      currentDirectionIndex = 0;
    }
    else
    {
      currentDirectionIndex++;
    }
  }
}

int p1 = config.BottomWallY - playGround.Min(x => x.Y);

sw.Stop();

Console.WriteLine($"Part one: {p1}");
Console.WriteLine($"Part two: {0}");
Console.WriteLine($"Time: {sw.Elapsed}");


List<Shape> InitShapes()
{
  return new List<Shape>
  {
    new("Line hor.",
      -2,
      1,
      new Point(0, 0),
      new Point(1, 0),
      new Point(2, 0),
      new Point(3, 0)
    ),

    new("Cross",
      -2,
      2,
      new Point(1, 0),
      new Point(1, 1),
      new Point(1, 2),
      new Point(0, 1),
      new Point(2, 1)
    ),

    new("L reverted",
      -2,
      2,
      new Point(2, 0),
      new Point(2, 1),
      new Point(2, 2),
      new Point(1, 2),
      new Point(0, 2)
    ),

    new("Line ver.",
      -2,
      4,
      new Point(0, 0),
      new Point(0, 1),
      new Point(0, 2),
      new Point(0, 3)
    ),

    new("Square",
      -2,
      3,
      new Point(0, 0),
      new Point(1, 0),
      new Point(0, 1),
      new Point(1, 1)
    ),
  };
}

void DrawPlayground()
{
  if (!playGround.Any()) return;
  int minY = playGround.Min(x => x.Y);
  for (int y = minY; y <= config.BottomWallY; y++)
  {
    for (int x = config.LeftWallX; x <= config.RightWallX; x++)
    {
      if (y == config.BottomWallY)
      {
        if (x == config.LeftWallX || x == config.RightWallX) Console.Write("+");
        else Console.Write("-");
      }
      else if (x == config.LeftWallX || x == config.RightWallX) Console.Write("|");
      else
      {
        if (playGround.Contains(new Point(x, y))) Console.Write("#");
        else Console.Write(".");
      }
    }

    Console.WriteLine();
  }

  Console.WriteLine();
}