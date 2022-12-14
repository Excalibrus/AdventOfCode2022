namespace Shared.Objects;

public class MatrixPosition : IComparable
{
  public int Row { get; set; }
  public int Col { get; set; }

  public MatrixPosition(int row, int col)
  {
    Row = row;
    Col = col;
  }

  public int CompareTo(object obj)
  {
    if (obj is MatrixPosition position)
    {
      return position.Col == Col && position.Row == Row ? 0 : 1;
    }

    return 1;
  }

  public static bool operator ==(MatrixPosition obj1, MatrixPosition obj2)
  {
    return obj1.Row == obj2.Row && obj1.Col == obj2.Col;
  }

  public static bool operator !=(MatrixPosition obj1, MatrixPosition obj2)
  {
    if (obj1 is null && obj2 is null) return false;
    if (obj1 is null || obj2 is null) return true;
    return  obj1.Row != obj2.Row || obj1.Col != obj2.Col;
  }

  public override bool Equals(object? obj)
  {
    if (obj == null || GetType() != obj.GetType())
    {
      return false;
    }

    MatrixPosition pos = (MatrixPosition)obj;
    return Row == pos.Row && Col == pos.Col;
  }

  public override int GetHashCode()
  {
    return (Row, Col).GetHashCode();
  }
  
  public override string ToString()
  {
    return $"Row: {Row}, Col: {Col}";
  }
}

public class MatrixPositionComparer : IEqualityComparer<MatrixPosition>
{
  public bool Equals(MatrixPosition x, MatrixPosition y)
  {
    return x.Equals(y);
  }

  public int GetHashCode(MatrixPosition obj)
  {
    return obj.GetHashCode();
  }
}