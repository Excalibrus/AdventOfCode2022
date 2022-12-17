using System.Diagnostics;
using Day17;
using Shared;

Stopwatch sw = new();
sw.Start();
FileReader reader = new("input.txt");

List<Direction> directions = reader.ReadStringLines().ParseDirections();
List<Shape> shapes = InitShapes();

int currentShapeIndex = 0;
int currentDirectionIndex = 0;
long iterations = 4000;
List<int> heightsAfterShape = new();
long target = 1_000_000_000_000;
// long target = 2022;
// long iterations = 2022;
// long iterations = 1_000_000_000_000;
// long iterations = 10_000;

List<PointWithType> playGround = new();

WallConfig config = new()
{
  LeftWallX = -1,
  RightWallX = 7,
  BottomWallY = 3000,
  InitialWinds = 4
};

List<int> newRoundYCoordinates = new();
Dictionary<int, List<PointWithType>> playGroundHistory = new();
for (long i = 1; i <= iterations; i++)
{
  if (i % 1000 == 0)
  {
    Console.WriteLine(i);
  }

  if (currentShapeIndex == 0)
  {
    // DrawPlayground();
  }

  bool fillPlayGround = true;
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
            new PointWithType(windPoint.X + (direction == Direction.Left ? -1 : 1), windPoint.Y, ShapeType.Cross)
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
          .Contains(new PointWithType(landingPoint.X, landingPoint.Y + 1, ShapeType.Cross)) ||
        config.BottomWallY - 1 == landingPoint.Y
      );

    if (canLand)
    {
      // landed
      if (currentShape.Type == ShapeType.HorizontalLine)
      {
        newRoundYCoordinates.Add(currentShape.Points.First().Y);
        playGroundHistory.Add(newRoundYCoordinates.Count - 1, new List<PointWithType>());
      }

      playGroundHistory[newRoundYCoordinates.Count - 1].AddRange(currentShape.Points);

      playGround.AddRange(currentShape.Points);
      heightsAfterShape.Add(playGround.Max(x => x.Y) - playGround.Min(x => x.Y) + 1);
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

List<(int from, int to)> foundPairs = new();
int minPatternRangeSize = 10;
int? comparedIndex = null;
int? foundIndex = null;
bool cancel = false;
for (int i = 0; i < playGroundHistory.Count - 1; i++)
{
  if (cancel) break;
  // cancel = false;
  for (int j = i + 1; j < playGroundHistory.Count; j++)
  {
    if (cancel) break;
    for (int pointIndex = 0; pointIndex < playGroundHistory[i].Count; pointIndex++)
    {
      if (playGroundHistory[i].Count != playGroundHistory[j].Count ||
          playGroundHistory[i][pointIndex].X != playGroundHistory[j][pointIndex].X)
      {
        break;
      }

      if (pointIndex == playGroundHistory[i].Count - 1 && j - i > minPatternRangeSize)
      {
        foundPairs.Add((i,j));
        // comparedIndex = i;
        // foundIndex = j;
        // break;
        // cancel = true;
      }
    }
  }
}

foreach ((int from, int to) in foundPairs)
{
  int starting = from * shapes.Count;
  long tmp = target - starting;
  long step = (to - from) * shapes.Count;
  if (tmp % step == 0)
  {
    comparedIndex = from;
    foundIndex = to;
    break;
  }
}
if (!comparedIndex.HasValue || !foundIndex.HasValue)
{
  throw new Exception("Pattern not found");
}

long baseHeight = Math.Abs(config.BottomWallY - playGroundHistory[comparedIndex.Value - 1].Min(x => x.Y));
List<PointWithType> patternRange = playGroundHistory
  .Where(x => x.Key >= comparedIndex && x.Key < foundIndex.Value)
  .SelectMany(x => x.Value)
  .ToList();

// normalize height
// int maxY = patternRange.Max(x => x.Y);
// List<Point> normalizedPattern = patternRange
//   .Select(point => new Point(point.X, point.Y - maxY))
//   .ToList();

int patternHeight = Math.Abs(patternRange.Max(x => x.Y) - patternRange.Min(x => x.Y) + 1);

int indexStep = (foundIndex.Value - comparedIndex.Value) * shapes.Count;
long index = comparedIndex.Value * shapes.Count;

while (index < target)
{
  if (index < target)
  {
    baseHeight += patternHeight;
  }
  index += indexStep;
  
}

if (index > target)
{
  index -= indexStep;
  long missing = target - index;
  int historyIndex = comparedIndex.Value;
  int historyShapeIndex = 0;
  List<PointWithType> missingPoints = new();
  for (long i = 0; i < missing; i++)
  {
    missingPoints.AddRange(playGroundHistory[historyIndex].Where(x => x.ShapeType == shapes[historyShapeIndex].Type));
    if (historyShapeIndex == shapes.Count - 1)
    {
      historyShapeIndex = 0;
      historyIndex++;
    }
    else
    {
      historyShapeIndex++;
    }
  }

  int missingPointsSum = missingPoints.Max(x => x.Y) - missingPoints.Min(x => x.Y) + 1;
  baseHeight += missingPointsSum;
}

// int heightMultiplier = (target - baseHeight) / patternHeight;
// long height = baseHeight + patternHeight * heightMultiplier:
sw.Stop();

long p2 = baseHeight;

Console.WriteLine($"Part one: {p1}");
Console.WriteLine($"Part two: {p2}");
Console.WriteLine($"Time: {sw.Elapsed}");


List<Shape> InitShapes()
{
  return new List<Shape>
  {
    new(ShapeType.HorizontalLine,
      -2,
      1,
      new Point(0, 0),
      new Point(1, 0),
      new Point(2, 0),
      new Point(3, 0)
    ),

    new(ShapeType.Cross,
      -2,
      2,
      new Point(1, 0),
      new Point(1, 1),
      new Point(1, 2),
      new Point(0, 1),
      new Point(2, 1)
    ),

    new(ShapeType.RevertedL,
      -2,
      2,
      new Point(2, 0),
      new Point(2, 1),
      new Point(2, 2),
      new Point(1, 2),
      new Point(0, 2)
    ),

    new(ShapeType.VerticalLine,
      -2,
      4,
      new Point(0, 0),
      new Point(0, 1),
      new Point(0, 2),
      new Point(0, 3)
    ),

    new(ShapeType.Square,
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
      else if (x == config.LeftWallX || x == config.RightWallX)
      {
        Console.Write("|");
        if (x == config.RightWallX && newRoundYCoordinates.Contains(y))
        {
          Console.Write(newRoundYCoordinates.IndexOf(y));
        }
      }
      else
      {
        ConsoleColor color = Console.ForegroundColor;
        if (newRoundYCoordinates.Contains(y))
        {
          Console.ForegroundColor = ConsoleColor.Red;
        }

        if (playGround.Contains(new PointWithType(x, y, ShapeType.Cross))) Console.Write("#");
        else Console.Write(".");
        Console.ForegroundColor = color;
      }
    }

    Console.WriteLine();
  }

  Console.WriteLine();
}