using Shared.Enums;
using Shared.Objects;

namespace Shared.Extensions;

public static class MatrixExtensions
{
  public static List<MatrixPosition> GetNeighbourPositions<T>(this T[,] matrix, MatrixPosition currentPosition,
    Direction direction)
  {
    List<Direction> regularEnums = Enumerable.Range(0, 8).Select(x => (int)Math.Pow(2, x)).Select(x => (Direction)x).ToList();
    return Enum.GetValues(typeof(Direction))
      .Cast<Direction>()
      .Where(x => regularEnums.Contains(x) && direction.HasFlag(x))
      .Select(dir =>
        matrix.GetPositionsInDirection(currentPosition, dir, MatrixDepth.One)
          .FirstOrDefault())
      .Where(position => position != null).ToList();
  }


  public static List<MatrixPosition> GetPositionsInDirection<T>(
    this T[,] matrix,
    MatrixPosition currentPosition,
    Direction direction,
    MatrixDepth depth = MatrixDepth.Full)
  {
    List<MatrixPosition> positions = new();
    switch (direction)
    {
      case Direction.Up:
        for (int r = currentPosition.Row - 1; r >= 0; r--)
        {
          positions.Add(new MatrixPosition(r, currentPosition.Col));
          if (NeedToBreak(positions)) break;
        }

        break;
      case Direction.Down:
        for (int r = currentPosition.Row + 1; r < matrix.GetLength(0); r++)
        {
          positions.Add(new MatrixPosition(r, currentPosition.Col));
          if (NeedToBreak(positions)) break;
        }

        break;
      case Direction.Left:
        for (int c = currentPosition.Col - 1; c >= 0; c--)
        {
          positions.Add(new MatrixPosition(currentPosition.Row, c));
          if (NeedToBreak(positions)) break;
        }

        break;
      case Direction.Right:
        for (int c = currentPosition.Col + 1; c < matrix.GetLength(1); c++)
        {
          positions.Add(new MatrixPosition(currentPosition.Row, c));
          if (NeedToBreak(positions)) break;
        }

        break;
      case Direction.UpLeft:
        for (int r = currentPosition.Row - 1, c = currentPosition.Col - 1; r >= 0 && c >= 0; r--, c--)
        {
          positions.Add(new MatrixPosition(r, c));
          if (NeedToBreak(positions)) break;
        }

        break;
      case Direction.UpRight:
        for (int r = currentPosition.Row - 1, c = currentPosition.Col + 1; r >= 0 && c < matrix.GetLength(1); r--, c++)
        {
          positions.Add(new MatrixPosition(r, c));
          if (NeedToBreak(positions)) break;
        }

        break;
      case Direction.DownLeft:
        for (int r = currentPosition.Row + 1, c = currentPosition.Col - 1; r < matrix.GetLength(0) && c >= 0; r++, c--)
        {
          positions.Add(new MatrixPosition(r, c));
          if (NeedToBreak(positions)) break;
        }

        break;
      case Direction.DownRight:
        for (int r = currentPosition.Row + 1, c = currentPosition.Col + 1;
             r < matrix.GetLength(0) && c < matrix.GetLength(1);
             r++, c++)
        {
          positions.Add(new MatrixPosition(r, c));
          if (NeedToBreak(positions)) break;
        }

        break;
    }

    bool NeedToBreak(List<MatrixPosition> matrixPositions)
    {
      if (depth == MatrixDepth.One && matrixPositions.Count == 1) return true;
      if (depth == MatrixDepth.Two && matrixPositions.Count == 2) return true;
      return false;
    }

    return positions;
  }

  public static bool IsPositionOnEdge<T>(this T[,] matrix, MatrixPosition position)
  {
    return position.Row == 0 ||
           position.Col == 0 ||
           position.Row == matrix.GetLength(0) - 1 ||
           position.Col == matrix.GetLength(1) - 1;
  }
}