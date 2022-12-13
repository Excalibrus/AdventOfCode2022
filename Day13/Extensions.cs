using System.Text.Json.Nodes;

namespace Day13;

public static class Extensions
{
  public static List<Pair> ToPairs(this List<Item> items)
  {
    return items
      .Chunk(2)
      .Select(x => new Pair
      {
        Left = x.First(),
        Right = x.Last()
      }).ToList();
  }
  
  public static List<Item> ParseItems(this List<string> lines)
  {
    return lines
      .Where(x => !string.IsNullOrWhiteSpace(x))
      .Select(x => JsonNode.Parse(x).ParseItem())
      .ToList();
  }
  
  public static Result CompareWith(this Item leftItem, Item rightItem)
  {
    if (leftItem.IsInteger && rightItem.IsInteger)
    {
      if (leftItem.Number < rightItem.Number)
      {
        // Console.WriteLine($"Right order, left: {leftItem.Number}, right: {rightItem.Number}");
        return Result.RightOrder;
      }

      if (leftItem.Number > rightItem.Number)
      {
        // Console.WriteLine($"Wrong order, left: {leftItem.Number}, right: {rightItem.Number}");
        return Result.WrongOrder;
      }

      // Console.WriteLine($"Skipping, left: {leftItem.Number}, right: {rightItem.Number}");
      return Result.Skip;
    }

    if (!leftItem.IsInteger && !rightItem.IsInteger)
    {
      int smaller = leftItem.Items.Count > rightItem.Items.Count ? rightItem.Items.Count : leftItem.Items.Count;
      for (int i = 0; i < smaller; i++)
      {
        Result result = leftItem.Items[i].CompareWith(rightItem.Items[i]);
        if (result != Result.Skip)
        {
          return result;
        }
      }

      if (leftItem.Items.Count == rightItem.Items.Count) return Result.Skip;
      if (leftItem.Items.Count < rightItem.Items.Count) return Result.RightOrder;
      if (leftItem.Items.Count > rightItem.Items.Count) return Result.WrongOrder;
    }

    if (!leftItem.IsInteger && rightItem.IsInteger)
    {
      return leftItem.CompareWith(new Item { Items = { rightItem } });
    }

    return new Item { Items = { leftItem } }.CompareWith(rightItem);
  }


  private static Item ParseItem(this JsonNode node)
  {
    if (node is JsonArray arr)
    {
      return new Item()
      {
        Json = node.ToJsonString(),
        Items = arr.Select(x => x.ParseItem()).ToList()
      };
    }

    if (node is JsonValue val)
    {
      return new Item
      {
        IsInteger = true,
        Number = int.Parse(val.ToString())
      };
    }

    throw new Exception("Not found");
  }
}