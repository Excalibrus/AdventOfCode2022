using System.Diagnostics;
using Day13;
using Shared;


Stopwatch sw = new();
sw.Start();
FileReader reader = new("input.txt");

List<Pair> pairs = reader.ReadStringLines().ParsePairs();

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


sw.Stop();


Console.WriteLine($"Part one: {p1}");
Console.WriteLine($"Part two: {0}");
Console.WriteLine($"Time: {sw.Elapsed}");