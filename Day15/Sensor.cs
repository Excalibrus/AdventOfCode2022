using Shared;

namespace Day15;

public class Sensor: Point
{
  public string Id { get; set; } = Guid.NewGuid().ToString();
  public Sensor(int? x, int? y) : base(x, y)
  {
  }
}