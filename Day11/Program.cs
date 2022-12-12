using System.Diagnostics;
using System.Numerics;
using Day11;
using Shared;

Stopwatch sw = new();
sw.Start();
FileReader reader = new("input.txt");

List<Monkey> monkeysP1 = reader.ReadStringLines().ParseMonkeys();
long p1 = SolvePart(1, 20, monkeysP1, 0);
List<Monkey> monkeysP2 = reader.ReadStringLines().ParseMonkeys();
int factor = monkeysP2.Select(x => x.Divisible).Aggregate(1, (x, y) => x * y);
long p2 = SolvePart(2, 10000, monkeysP2, factor);

sw.Stop();
Console.WriteLine($"Part one: {p1}");
Console.WriteLine($"Part two: {p2}");
Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");

long SolvePart(int part, int rounds, List<Monkey> monkeys, int divFactor)
{
  for (int round = 1; round <= rounds; round++)
  {
    foreach (Monkey monkey in monkeys)
    {
      foreach ((int monkeyNumber, BigInteger item) in monkey.Inspect(part))
      {
        Monkey monkeyToAdd = monkeys.First(x => x.Number == monkeyNumber);
        if (divFactor > 0)
        {
          monkeyToAdd.Items.Add(item % divFactor);
        }
        else
        {
          monkeyToAdd.Items.Add(item);
        }
      }
    }

    if (round % 20 == 0)
    {
      // Console.WriteLine($"Round {round}");
      // Console.WriteLine($"First bigint: {monkeys.First().Items.First().GetBitLength()}");
    }

    // Console.WriteLine();
    if (round == 20 || round % 1000 == 0)
    {
      Console.WriteLine($"== After round {round} ==");
      foreach (Monkey monkey in monkeys)
      {
        Console.WriteLine($"Monkey {monkey.Number} inspected items {monkey.NumberOfInspections} times.");
      }
    }
  }

  return monkeys
    .Select(x => x.NumberOfInspections)
    .OrderByDescending(x => x)
    .Take(2)
    .Aggregate((x, y) => x * y);
}