namespace Shared;

[Flags]
public enum MatrixDirection
{
  Up = 1,
  UpRight,
  Right,
  DownRight,
  Down,
  DownLeft,
  Left,
  UpLeft,
  
  Cross = Up | Down | Right | Left,
  Diagonal = UpLeft | UpRight | DownLeft | DownRight,
  All = Cross | Diagonal
}