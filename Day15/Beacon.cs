using Shared;

namespace Day15;

public class Beacon: Point
{
  public string Id { get; set; } = Guid.NewGuid().ToString();
  public Beacon(int? x, int? y) : base(x, y)
  {
  }
  
  public Beacon(Point point) : base(point.X, point.Y)
  {
  }
}