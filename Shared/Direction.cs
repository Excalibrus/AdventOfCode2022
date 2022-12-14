namespace Shared;

[Flags]
public enum Direction
{
  Up = 1 << 0,
  UpRight = 1 << 1,
  Right = 1 << 2,
  DownRight = 1 << 3,
  Down = 1 << 4,
  DownLeft = 1 << 5,
  Left = 1 << 6,
  UpLeft = 1 << 7,
  
  Cross = Up | Down | Right | Left,
  Diagonal = UpLeft | UpRight | DownLeft | DownRight,
  All = Cross | Diagonal
}