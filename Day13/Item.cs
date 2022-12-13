namespace Day13;

public class Item
{
  public bool IsInteger { get; set; }
  public int? Number { get; set; }
  public List<Item> Items { get; set; } = new();
  public int? Index { get; set; }
}