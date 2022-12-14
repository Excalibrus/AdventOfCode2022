using Shared;

namespace Day17;

public static class ExtensionMethods
{
  public static List<Direction> ParseDirections(this List<string> lines)
  {
    return lines
      .SelectMany(x => x
        .Select(y => y == '<' ? Direction.Left : Direction.Right))
      .ToList();
  }

  public static Shape PrepareInitialShapePosition(
    this List<PointWithType> playground,
    Shape shape,
    WallConfig config,
    int spaceBetween = 3)
  {
    int leftXPosition = config.LeftWallX + 3;
    Shape initShape = shape.CreateCopy();
    initShape.MoveX(leftXPosition);

    int bottomPosition = playground.Any() ? playground.Min(x => x.Y) - (spaceBetween + 1) : config.BottomWallY - (spaceBetween + 1);

    int maxY = initShape.Points.Max(x => x.Y);
    int moveY = bottomPosition - maxY;
    initShape.MoveY(moveY);

    return initShape;
  }
}