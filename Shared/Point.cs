namespace Shared;

public class Point : IComparable
{
  public int X { get; set; }

  public int Y { get; set; }

  public Point(int? x, int? y)
  {
    X = x ?? 0;
    Y = y ?? 0;
  }

  public int CompareTo(object obj)
  {
    if (obj is Point point)
    {
      return point.X == X && point.Y == Y ? 0 : -1;
    }

    return 1;
  }

  public static bool operator ==(Point obj1, Point obj2)
  {
    return obj1.X == obj2.X && obj1.Y == obj2.Y;
  }

  public static bool operator !=(Point obj1, Point obj2)
  {
    if (obj1 is null && obj2 is null) return false;
    if (obj1 is null || obj2 is null) return true;
    return  obj1.X != obj2.X || obj1.Y != obj2.Y;
  }

  public override bool Equals(object? obj)
  {
    if (obj == null || GetType() != obj.GetType())
    {
      return false;
    }

    Point pos = (Point)obj;
    return X == pos.X && Y == pos.Y;
  }

  public override int GetHashCode()
  {
    return (X, Y).GetHashCode();
  }
  
  public override string ToString()
  {
    return $"X: {X}, Y: {Y}";
  }
}

public class PointComparer : IEqualityComparer<Point>
{
  public bool Equals(Point x, Point y)
  {
    return x.Equals(y);
  }

  public int GetHashCode(Point obj)
  {
    return obj.GetHashCode();
  }
}