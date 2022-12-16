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

int p1 = 0;
BruteForceToWin(currentValve, 1, 31, 0, new List<Valve> { currentValve }, new List<string>());

// for (int minute = 1; minute <= 30; minute++)
// {
//   Console.WriteLine($"== Minute {minute} ==");
//   if (!openedValves.Any())
//   {
//     Console.WriteLine("No valves are open.");
//   }
//   else if(openedValves.Count == 1)
//   {
//     Console.WriteLine($"Valve {openedValves.First().Name} is open, releasing {openedValves.First().Rate} pressure.");
//   }
//   else
//   {
//     Console.WriteLine($"Valves {string.Join(", ", openedValves.Select(x => x.Name))} are open, releasing {openedValves.Sum(x => x.Rate)} pressure.");
//   }
//   totalPressure += openedValves.Sum(x => x.Rate);
//
//   List<Valve> possibleNextValves = valves.Where(x => !x.IsOpened && currentValve.LeadsTo.Contains(x.Name)).ToList();
//   
//   Valve? nextPossible = null;
//   decimal rateMax = 0;
//   foreach (Valve possibleNextValve in possibleNextValves)
//   {
//     decimal maxPossRate = 0;
//     int minSteps = 0;
//     foreach (Valve valve in valves.Where(x => !x.IsOpened && x.Name != possibleNextValve.Name).OrderByDescending(x => x.Rate).Take(5))
//     {
//       (decimal rate, int steps) = CalculateRateForPath(possibleNextValve.Name, valve.Name);
//       decimal intmRate = rate + steps * valve.Rate;
//       if (intmRate > maxPossRate)
//       {
//         maxPossRate = intmRate;
//         minSteps = steps;
//       }
//     }
//
//     maxPossRate += minSteps * possibleNextValve.Rate;
//     if (maxPossRate > rateMax)
//     {
//       rateMax = maxPossRate;
//       nextPossible = possibleNextValve;
//     }
//   }
//   
//   
//   
//   // List<int> rates = paths.Select(x => CalculateRateForPath(currentValve.Name, x.Name)).ToList();
//   // Valve? betterValve = paths.MaxBy(x => CalculateRateForPath(currentValve.Name, x.Name));
//   // if (currentValve.IsOpened || currentValve.Rate == 0)
//   // {
//   //   betterValve = paths.MaxBy(x => FindMaxRateForPath(currentValve.Name, x.Name));
//   // }
//   // else
//   // {
//   //   List<Valve> orderedPaths = paths.OrderByDescending(x => x.Rate).ToList();
//   //   betterValve = orderedPaths.FirstOrDefault(x => x.Rate > currentValve.Rate * 3);
//   //   if (betterValve is null)
//   //   {
//   //     betterValve = orderedPaths.FirstOrDefault(x => x.GetChildRate(valves, 1) > currentValve.Rate * 2);
//   //   }
//   //   if (betterValve is null)
//   //   {
//   //     betterValve = orderedPaths.FirstOrDefault(x => x.GetChildRate(valves, 2) > currentValve.Rate);
//   //   }
//   //   if (betterValve is null)
//   //   {
//   //     betterValve = orderedPaths.FirstOrDefault();
//   //   }
//   // }
//   // Valve betterValve = null;
//   // if (betterValve != null)
//   // {
//   //   currentValve = betterValve;
//   //   Console.WriteLine($"You move to valve {betterValve.Name}.");
//   // }
//   // else if (currentValve.Rate == 0)
//   // {
//   //   currentValve = paths
//   //     .OrderByDescending(x => x.GetChildRate(valves, 2))
//   //     .ThenByDescending(x => x.GetChildRate(valves, 1))
//   //     .ThenByDescending(x => x.Rate)
//   //     .FirstOrDefault();
//   //   Console.WriteLine($"You move to valve {currentValve.Name}.");
//   // }
//   // else
//   // {
//   //   currentValve.IsOpened = true;
//   //   Console.WriteLine($"You open valve {currentValve.Name}.");
//   //   openedValves.Add(currentValve);
//   // }
//   Console.WriteLine();
// }

sw.Stop();

Console.WriteLine($"Part one: {p1}");
Console.WriteLine($"Part two: {0}");
Console.WriteLine($"Time: {sw.Elapsed}");

void BruteForceToWin(Valve current, int minute, int maxMinute, int currentPressure, List<Valve> currentPath,
  List<string> openedValves)
{
  Console.WriteLine(minute);
  if (minute == maxMinute)
  {
    if (currentPressure > p1)
    {
      p1 = currentPressure;
    }

    return;
  }

  int pressure = openedValves.Select(x => valves.First(v => v.Name == x)).Sum(x => x.Rate);
  foreach (string leadsTo in current.LeadsTo)
  {
    Valve valve = valves.First(x => x.Name == leadsTo);
    BruteForceToWin(
      valve,
      minute + 1,
      maxMinute,
      currentPressure + pressure,
      new List<Valve>(currentPath) { valve },
      new List<string>(openedValves));
  }

  BruteForceToWin(
    currentValve,
    minute + 1,
    maxMinute,
    currentPressure + pressure,
    new List<Valve>(currentPath) { currentValve },
    new List<string>(openedValves) { current.Name });
}

(decimal rate, int steps) CalculateRateForPath(string from, string to)
{
  decimal maxRate = 0;
  int steps = 0;
  foreach (List<Valve> valves in allValvePaths[(from, to)].Where(x => x.All(x => !x.IsOpened)))
  {
    decimal fullValveRate = 0;
    List<int> previousValveRates = new();
    foreach (Valve valve in valves)
    {
      previousValveRates.Add(valve.Rate * 2);
      fullValveRate += previousValveRates.Sum();
    }

    if (fullValveRate > maxRate)
    {
      steps = valves.Count;
      maxRate = fullValveRate;
    }
  }

  return (maxRate, steps);
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