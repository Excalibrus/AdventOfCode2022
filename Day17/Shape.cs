using Shared;

namespace Day17;

public class Shape
{
  public string Name { get; set; }
  public List<Point> Points { get; set; } = new();
  public int InitialXPosition { get; set; } = 0;
  public int MaxLeftPositionX { get; set; }
  public int MaxRightPositionX { get; set; }

  public Shape(string name, int possibleLeft, int possibleRight, params Point[] points)
  {
    MaxLeftPositionX = possibleLeft;
    MaxRightPositionX = possibleRight;
    Name = name;
    Points = points.ToList();
  }

  public Shape CreateCopy()
  {
    return new Shape(Name, MaxLeftPositionX, MaxRightPositionX, Points.Select(x => new Point(x.X, x.Y)).ToArray());
  }

  public void MoveX(int xPositions)
  {
    foreach (Point point in Points)
    {
      point.X += xPositions;
    }
  }
  
  public void MoveY(int yPositions)
  {
    foreach (Point point in Points)
    {
      point.Y += yPositions;
    }
  }

  public IEnumerable<Point> LandingPoints()
  {
    int minX = Points.Min(x => x.X);
    int maxX = Points.Max(x => x.X);
    for (int x = minX; x <= maxX; x++)
    {
      yield return Points.Where(point => point.X == x).MaxBy(point => point.Y);
    }
  }
  
  public IEnumerable<Point> WindPoints(Direction direction)
  {
    int minY = Points.Min(x => x.Y);
    int maxY = Points.Max(x => x.Y);
    for (int y = minY; y <= maxY; y++)
    {
      List<Point> verticalPoints = Points.Where(point => point.Y == y).ToList();
      yield return direction == Direction.Right
        ? verticalPoints.MaxBy(point => point.X)
        : verticalPoints.MinBy(point => point.X);
    }
  }
}