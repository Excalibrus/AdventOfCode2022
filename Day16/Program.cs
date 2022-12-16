using System.Diagnostics;
using Day16;
using Shared;

Stopwatch sw = new();
sw.Start();
FileReader reader = new("input_demo.txt");

List<Valve> valves = reader.ReadStringLines().ParseValves().ToList();

Valve currentValve = valves.First();
int totalPressure = 0;
List<Valve> openedValves = new();

Dictionary<(string from, string to), List<List<Valve>>> allValvePaths = new();
foreach (Valve fromValve in valves)
{
  foreach (Valve toValve in valves)
  {
    if (fromValve.Name != toValve.Name)
    {
      allValvePaths.Add((fromValve.Name, toValve.Name), new List<List<Valve>>());
      FindPath(fromValve.Name, fromValve.Name, toValve.Name, valves, new List<Valve>());  
    }
  }
}



for (int minute = 1; minute <= 30; minute++)
{
  Console.WriteLine($"== Minute {minute} ==");
  if (!openedValves.Any())
  {
    Console.WriteLine("No valves are open.");
  }
  else if(openedValves.Count == 1)
  {
    Console.WriteLine($"Valve {openedValves.First().Name} is open, releasing {openedValves.First().Rate} pressure.");
  }
  else
  {
    Console.WriteLine($"Valves {string.Join(", ", openedValves.Select(x => x.Name))} are open, releasing {openedValves.Sum(x => x.Rate)} pressure.");
  }
  totalPressure += openedValves.Sum(x => x.Rate);

  Valve? nextPossible = null;
  foreach (Valve valve in valves.Where(x => !x.IsOpened).OrderByDescending(x => x.Rate))
  {
    int rate = CalculateRateForPath(currentValve.Name, valve.Name);
  }
  
  List<Valve> paths = valves.Where(x => !x.IsOpened && currentValve.LeadsTo.Contains(x.Name)).ToList();
  List<int> rates = paths.Select(x => CalculateRateForPath(currentValve.Name, x.Name)).ToList();
  Valve? betterValve = paths.MaxBy(x => CalculateRateForPath(currentValve.Name, x.Name));
  // if (currentValve.IsOpened || currentValve.Rate == 0)
  // {
  //   betterValve = paths.MaxBy(x => FindMaxRateForPath(currentValve.Name, x.Name));
  // }
  // else
  // {
  //   List<Valve> orderedPaths = paths.OrderByDescending(x => x.Rate).ToList();
  //   betterValve = orderedPaths.FirstOrDefault(x => x.Rate > currentValve.Rate * 3);
  //   if (betterValve is null)
  //   {
  //     betterValve = orderedPaths.FirstOrDefault(x => x.GetChildRate(valves, 1) > currentValve.Rate * 2);
  //   }
  //   if (betterValve is null)
  //   {
  //     betterValve = orderedPaths.FirstOrDefault(x => x.GetChildRate(valves, 2) > currentValve.Rate);
  //   }
  //   if (betterValve is null)
  //   {
  //     betterValve = orderedPaths.FirstOrDefault();
  //   }
  // }
  if (betterValve != null)
  {
    currentValve = betterValve;
    Console.WriteLine($"You move to valve {betterValve.Name}.");
  }
  else if (currentValve.Rate == 0)
  {
    currentValve = paths
      .OrderByDescending(x => x.GetChildRate(valves, 2))
      .ThenByDescending(x => x.GetChildRate(valves, 1))
      .ThenByDescending(x => x.Rate)
      .FirstOrDefault();
    Console.WriteLine($"You move to valve {currentValve.Name}.");
  }
  else
  {
    currentValve.IsOpened = true;
    Console.WriteLine($"You open valve {currentValve.Name}.");
    openedValves.Add(currentValve);
  }
  Console.WriteLine();
}

sw.Stop();

Console.WriteLine($"Part one: {totalPressure}");
Console.WriteLine($"Part two: {0}");
Console.WriteLine($"Time: {sw.Elapsed}");

int CalculateRateForPath(string from, string to)
{
  int maxRate = 0;
  foreach (List<Valve> valves in allValvePaths[(from, to)].Where(x => x.All(x => !x.IsOpened)))
  {
    int fullValveRate = 0;
    List<int> previousValveRates = new();
    foreach (Valve valve in valves)
    {
      previousValveRates.Add(valve.Rate);
      fullValveRate += previousValveRates.Sum();
    }

    if (fullValveRate > maxRate)
    {
      maxRate = fullValveRate;
    }
  }

  return maxRate;
}

void FindPath(string original, string from, string to, List<Valve> valves, List<Valve> currentPath, int depth = 1)
{
  if (depth >= valves.Count) return;
  Valve? fromValve = valves.FirstOrDefault(x => x.Name == from && !x.IsOpened);
  if (fromValve is null) return;
  
  currentPath.Add(fromValve);
  foreach (string currentName in fromValve.LeadsTo.Where(x => currentPath.All(v => v.Name != x)))
  {
    if (currentName.Equals(to))
    {
      currentPath.Add(valves.First(x => x.Name == to));
      allValvePaths[(original, to)].Add(currentPath);
    }

    FindPath(original, currentName, to, valves, new List<Valve>(currentPath), depth + 1);
  }
}