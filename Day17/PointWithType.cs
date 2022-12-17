using Shared;

namespace Day17;

public class PointWithType : Point
{
  public ShapeType ShapeType { get; set; }
  public string MatchingKey { get; set; }
  
  public PointWithType(int? x, int? y, ShapeType type) : base(x, y)
  {
    ShapeType = type;
  }

  public PointWithType(Point point, ShapeType type) : base(point.X, point.Y)
  {
    ShapeType = type;
  }
}