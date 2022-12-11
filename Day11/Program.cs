using System.Diagnostics;
using Day11;
using Shared;

Stopwatch sw = new();
sw.Start();
FileReader reader = new("input.txt");

List<Monkey> monkeys = reader.ReadStringLines().ParseMonkeys();
int roundsLimit = 20;

for (int i = 1; i <= roundsLimit; i++)
{
  foreach (Monkey monkey in monkeys)
  {
    foreach ((int monkeyNumber, int item) in monkey.Inspect())
    {
      monkeys.First(x => x.Number == monkeyNumber).Items.Add(item);
    }
  }
}

int p1 = monkeys
  .Select(x => x.NumberOfInspections)
  .OrderByDescending(x => x)
  .Take(2)
  .Aggregate((x, y) => x * y);

sw.Stop();
Console.WriteLine($"Part one: {p1}");
Console.WriteLine($"Part two: {0}");
Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");