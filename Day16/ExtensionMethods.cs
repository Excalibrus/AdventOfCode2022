namespace Day16;

public static class ExtensionMethods
{
  public static IEnumerable<Valve> ParseValves(this List<string> lines)
  {
    foreach (string line in lines)
    {
      string lastWords = line.Contains("valves") ? "; tunnels lead to valves " : "; tunnel leads to valve ";
      string[] parts = line.Split(new[]
        {
          "Valve ",
          " has flow rate=",
          lastWords
        },
        StringSplitOptions.RemoveEmptyEntries);
      yield return new Valve
      {
        Name = parts[0],
        Rate = int.Parse(parts[1]),
        LeadsTo = parts[2]
          .Replace(",", "")
          .Split(" ")
          .Where(x => x.Length > 1)
          .ToList()
      };
    }
  }
}