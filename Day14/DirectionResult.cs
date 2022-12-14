using Shared;

namespace Day14;

public class DirectionResult
{
  public MaterialPoint OriginalPoint { get; set; }
  public MaterialPoint? NextMaterial { get; set; }
  public Point NextPoint { get; set; }
  public bool Continue => NextMaterial is null;
}