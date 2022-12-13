using System.Diagnostics;
using Day13;
using Shared;


Stopwatch sw = new();
sw.Start();
FileReader reader = new("input.txt");
List<string> inputLines = reader.ReadStringLines();
List<Pair> pairs = inputLines.ParseItems().ToPairs();

List<int> results = new();
for (int index = 0; index < pairs.Count; index++)
{
  Result result = pairs[index].Left.CompareWith(pairs[index].Right);
  // Console.WriteLine($"Pair result {result.ToString()}");
  if (result == Result.RightOrder)
  {
    results.Add(index + 1);
  }
}

int p1 = results.Aggregate((x, y) => x + y);

List<Item> dividerItems = new List<string>
{
  "[[2]]",
  "[[6]]"
}.ParseItems();


List<Item> items = inputLines.ParseItems().Concat(dividerItems).ToList();

items.Sort((x, y) => x.CompareWith(y) == Result.RightOrder ? -1 : 1);

// Console.WriteLine("Ordered");
// foreach (Item item in items)
// {
//   Console.WriteLine(item.Json);
// }

int firstDividerIndex = items.FindIndex(x => x.Id == dividerItems.First().Id) + 1;
int secondDividerIndex = items.FindIndex(x => x.Id == dividerItems.Last().Id) + 1;

int p2 = firstDividerIndex * secondDividerIndex;

sw.Stop();


Console.WriteLine($"Part one: {p1}");
Console.WriteLine($"Part two: {p2}");
Console.WriteLine($"Time: {sw.Elapsed}");