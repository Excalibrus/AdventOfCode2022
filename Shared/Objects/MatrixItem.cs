namespace Shared.Objects;

public class MatrixPosition
{
  public int Row { get; set; }
  public int Col { get; set; }

  public MatrixPosition(int row, int col)
  {
    Row = row;
    Col = col;
  }
}