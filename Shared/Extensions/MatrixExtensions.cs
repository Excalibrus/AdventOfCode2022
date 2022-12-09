using Shared.Enums;
using Shared.Objects;

namespace Shared.Extensions;

public static class MatrixExtensions
{
  public static List<MatrixPosition> GetNeighbourPositions(this int[,] matrix, MatrixPosition currentPosition,
    bool includeDiagonal)
  {
    List<MatrixPosition> positions = new();
    positions.Add(matrix.GetPositionsInDirection(currentPosition, MatrixDirection.Up, MatrixDepth.One).First());
    positions.Add(matrix.GetPositionsInDirection(currentPosition, MatrixDirection.Down, MatrixDepth.One).First());
    positions.Add(matrix.GetPositionsInDirection(currentPosition, MatrixDirection.Left, MatrixDepth.One).First());
    positions.Add(matrix.GetPositionsInDirection(currentPosition, MatrixDirection.Right, MatrixDepth.One).First());
    if (includeDiagonal)
    {
      positions.Add(matrix.GetPositionsInDirection(currentPosition, MatrixDirection.UpLeft, MatrixDepth.One).First());
      positions.Add(matrix.GetPositionsInDirection(currentPosition, MatrixDirection.UpRight, MatrixDepth.One).First());
      positions.Add(matrix.GetPositionsInDirection(currentPosition, MatrixDirection.DownLeft, MatrixDepth.One).First());
      positions.Add(matrix.GetPositionsInDirection(currentPosition, MatrixDirection.DownRight, MatrixDepth.One).First());
    }

    return positions;
  }


  public static List<MatrixPosition> GetPositionsInDirection(
    this int[,] matrix,
    MatrixPosition currentPosition,
    MatrixDirection direction,
    MatrixDepth depth = MatrixDepth.Full)
  {
    List<MatrixPosition> positions = new();
    switch (direction)
    {
      case MatrixDirection.Up:
        for (int r = currentPosition.Row - 1; r >= 0; r--)
        {
          positions.Add(new MatrixPosition(r, currentPosition.Col));
          if (NeedToBreak(positions)) break;
        }

        break;
      case MatrixDirection.Down:
        for (int r = currentPosition.Row + 1; r < matrix.GetLength(0); r++)
        {
          positions.Add(new MatrixPosition(r, currentPosition.Col));
          if (NeedToBreak(positions)) break;
        }

        break;
      case MatrixDirection.Left:
        for (int c = currentPosition.Col - 1; c >= 0; c--)
        {
          positions.Add(new MatrixPosition(currentPosition.Row, c));
          if (NeedToBreak(positions)) break;
        }

        break;
      case MatrixDirection.Right:
        for (int c = currentPosition.Col + 1; c < matrix.GetLength(1); c++)
        {
          positions.Add(new MatrixPosition(currentPosition.Row, c));
          if (NeedToBreak(positions)) break;
        }

        break;
      case MatrixDirection.UpLeft:
        for (int r = currentPosition.Row - 1, c = currentPosition.Col - 1; r >= 0 && c >= 0; r--, c--)
        {
          positions.Add(new MatrixPosition(r, c));
          if (NeedToBreak(positions)) break;
        }

        break;
      case MatrixDirection.UpRight:
        for (int r = currentPosition.Row - 1, c = currentPosition.Col + 1; r >= 0 && c < matrix.GetLength(1); r--, c++)
        {
          positions.Add(new MatrixPosition(r, c));
          if (NeedToBreak(positions)) break;
        }

        break;
      case MatrixDirection.DownLeft:
        for (int r = currentPosition.Row + 1, c = currentPosition.Col - 1; r < matrix.GetLength(0) && c >= 0; r++, c--)
        {
          positions.Add(new MatrixPosition(r, c));
          if (NeedToBreak(positions)) break;
        }

        break;
      case MatrixDirection.DownRight:
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

  public static bool IsPositionOnEdge(this int[,] matrix, MatrixPosition position)
  {
    return position.Row == 0 ||
           position.Col == 0 ||
           position.Row == matrix.GetLength(0) - 1 ||
           position.Col == matrix.GetLength(1) - 1;
  }
}