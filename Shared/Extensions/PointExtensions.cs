namespace Shared.Extensions;

public static class PointExtensions
{
  public static List<Point> GetNeighbours(this Point point, Direction direction = Direction.All)
  {
    List<Direction> regularEnums =
      Enumerable.Range(0, 8).Select(x => (int)Math.Pow(2, x)).Select(x => (Direction)x).ToList();
    return Enum.GetValues(typeof(Direction))
      .Cast<Direction>()
      .Where(x => regularEnums.Contains(x) && direction.HasFlag(x))
      .Select(point.GoInDirection)
      .ToList();
  }

  public static Point GoInDirection(this Point point, Direction direction)
  {
    return direction switch
    {
      Direction.Up => new Point(point.X, point.Y - 1),
      Direction.Down => new Point(point.X, point.Y + 1),
      Direction.Left => new Point(point.X - 1, point.Y),
      Direction.Right => new Point(point.X + 1, point.Y),
      Direction.UpLeft => new Point(point.X - 1, point.Y - 1),
      Direction.UpRight => new Point(point.X + 1, point.Y - 1),
      Direction.DownRight => new Point(point.X + 1, point.Y + 1),
      Direction.DownLeft => new Point(point.X - 1, point.Y + 1),
      _ => throw new Exception($"Invalid direction {direction}")
    };
  }

  public static List<Point> GetPointsBetween(this Point point, Point comparingPoint)
  {
    List<Point> pointsBetween = new();
    if (point.X == comparingPoint.X && point.Y == comparingPoint.Y)
    {
      return pointsBetween;
    }
    if (point.X == comparingPoint.X)
    {
      int minPoint = point.Y < comparingPoint.Y ? point.Y : comparingPoint.Y;
      int maxPoint = point.Y > comparingPoint.Y ? point.Y : comparingPoint.Y;
      for (int y = minPoint + 1; y < maxPoint; y++)
      {
        pointsBetween.Add(new Point(point.X, y));
      }
    }
    else if (point.Y == comparingPoint.Y)
    {
      int minPoint = point.X < comparingPoint.X ? point.X : comparingPoint.X;
      int maxPoint = point.X > comparingPoint.X ? point.X : comparingPoint.X;
      for (int x = minPoint + 1; x < maxPoint; x++)
      {
        pointsBetween.Add(new Point(x, point.Y));
      }
    }
    else
    {
      throw new Exception("Could not get points between two points that are not horizontally or vertically aligned.");
    }

    return pointsBetween;
  }
}