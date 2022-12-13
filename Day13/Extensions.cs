using System.Text.Json.Nodes;

namespace Day13;

public static class Extensions
{
  public static List<Pair> ParsePairs(this List<string> lines)
  {
    return lines
      .Where(x => !string.IsNullOrWhiteSpace(x))
      .Chunk(2)
      .Select(x => x.Take(2).Select(x => JsonNode.Parse(x)))
      .ToList()
      .Select(x => new Pair
      {
        Left = x.First().ParseItem(),
        Right = x.Last().ParseItem()
      }).ToList();
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

  // private static List<Item> ParseItems(this string input, List<Item> items = new())
  // {
  //   char letter = input.First();
  //   string nextInput = input[1..];
  //   if (string.IsNullOrWhiteSpace(nextInput))
  //   {
  //     Console.WriteLine("ENDING");
  //     return items;
  //   }
  //   if (letter == '[')
  //   {
  //     items.Add(new Item
  //     {
  //       Items = nextInput.ParseItems()
  //     });
  //   }
  //   else if (letter == ']')
  //   {
  //     return items;
  //   }
  //   else if (letter == ',')
  //   {
  //     
  //   }
  // }

  private static Item ParseItem(this JsonNode node)
  {
    if (node is JsonArray arr)
    {
      return new Item()
      {
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