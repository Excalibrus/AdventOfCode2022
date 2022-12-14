using Shared;

namespace Day14;

public class MaterialPoint: Point
{
  public MaterialType Material { get; set; }

  public MaterialPoint(int? x, int? y, MaterialType? material) : base(x, y)
  {
    Material = material ?? MaterialType.Sand;
  }
  
  public MaterialPoint(Point point, MaterialType? material) : base(point.X, point.Y)
  {
    Material = material ?? MaterialType.Sand;
  }
}