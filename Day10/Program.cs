using System.Diagnostics;
using Shared;

Stopwatch sw = new();
sw.Start();
FileReader reader = new("input.txt");
List<string> lines = reader.ReadStringLines();

int x = 1;
int currentCycle = 0;

int addxCycles = lines.Count(x => x != "noop") * 2;
int totalCycles = addxCycles + (addxCycles - lines.Count);
List<int> signalStrengths = new();

List<int> checkCycles = Enumerable.Range(0, totalCycles / 40 + 2)
  .Select(n => (n * 40) + 20)
  .ToList();

foreach (string line in lines)
{
  if (line.Equals("noop"))
  {
    currentCycle++;
    CheckStrength();
  }
  else if (int.TryParse(line.Split(" ").Last(), out int adding))
  {
    currentCycle++;
    CheckStrength();
    currentCycle++;
    CheckStrength();
    x += adding;
  }
}

void CheckStrength()
{
  if (checkCycles.Contains(currentCycle))
  {
    signalStrengths.Add(currentCycle * x);
  }
}

foreach (int signalStrength in signalStrengths)
{
  Console.WriteLine(signalStrength);
}

int p1 = signalStrengths.Sum();


sw.Stop();
Console.WriteLine($"Part one: {p1}");
Console.WriteLine($"Part two: {0}");
Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");