using System.Xml.Linq;

namespace Day16;

public class Valve
{
  public string Name { get; set; }
  public int Rate { get; set; }
  public bool IsOpened { get; set; }
  public List<string> LeadsTo { get; set; } = new();

  public int GetChildRate(List<Valve> valves, int depth)
  {
    if (depth == 1)
    {
      return valves.Where(x => LeadsTo.Contains(x.Name) && !x.IsOpened).Sum(x => x.Rate);
    }

    if (depth == 2)
    {
      List<string> visited = new();
      List<Valve> visitedValves = new();
      int sum = 0;
      foreach (Valve valve in valves.Where(x => !x.IsOpened && LeadsTo.Contains(x.Name) && !visited.Contains(x.Name)))
      {
        sum += valve.Rate * depth;
        visited.Add(valve.Name);
        visitedValves.Add(valve);
      }

      foreach (Valve valve in visitedValves.ToList())
      {
        List<Valve> childValves = valves
          .Where(x => !x.IsOpened && valve.LeadsTo.Contains(x.Name) && !visited.Contains(x.Name)).ToList();
        foreach (Valve childValve in childValves)
        {
          sum += childValve.Rate;
          visited.Add(childValve.Name);
          visitedValves.Add(childValve);
        }
      }

      return sum;
    }

    return 0;
  }
}