using System.Diagnostics;
using Shared;

Stopwatch sw = new();
sw.Start();
FileReader reader = new();
List<string> lines = reader.ReadStringLines();

List<string> stacks = new();
List<string> stacks2 = new();
foreach (string line in lines)
{
  // parsing stacks
  if (line.Contains("["))
  {
    int num = 1;
    int i = 1;
    while (i < line.Length)
    {
      if (stacks.Count < num)
      {
        stacks.Add(string.Empty);
        stacks2.Add(string.Empty);
      }
      if (!string.IsNullOrWhiteSpace(line[i].ToString()))
      {
        stacks[num-1] = line[i] + stacks[num-1];
        stacks2[num-1] = line[i] + stacks2[num-1];
      }

      i += 4;
      num++;
    }
  }
  else if (line.StartsWith("move"))
  {
    int.TryParse(line.Split("from")[0].Replace("move", "").Trim(), out int repetitions);
    int.TryParse(line.Split("from")[1].Split("to")[0].Trim(), out int from);
    int.TryParse(line.Split("from")[1].Split("to")[1].Trim(), out int to);

    for (int i = 0; i < repetitions; i++)
    {
      stacks[to - 1] += stacks[from - 1].Last();
      stacks[from - 1] = stacks[from - 1][..^1];
    }

    string movingPart = stacks2[from - 1][(stacks2[from - 1].Length - repetitions)..];
    stacks2[to - 1] += movingPart;
    stacks2[from - 1] = stacks2[from - 1][..^movingPart.Length]; 
  }
}

string resultP1 = string.Join("", stacks.Select(x => x.Length > 0 ? x.Last().ToString() : " "));
string resultP2 = string.Join("", stacks2.Select(x => x.Length > 0 ? x.Last().ToString() : " "));

sw.Stop();
Console.WriteLine($"Part one: {resultP1}");
Console.WriteLine($"Part two: {resultP2}");
Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");