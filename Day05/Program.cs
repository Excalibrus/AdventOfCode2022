using System.Diagnostics;
using Shared;

// Day05.Day5.Solve();
// return;

Stopwatch sw = new();
sw.Start();
FileReader reader = new("input.txt");
List<string> lines = reader.ReadStringLines();

int lastStackModificationNum = 0;
int lastStackNumOfItems = 0;
int counter = 1;

bool isP1 = false;
List<string> stacks = new();
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
      }
      if (!string.IsNullOrWhiteSpace(line[i].ToString()))
      {
        stacks[num-1] = line[i].ToString() + stacks[num-1];
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

    if (isP1)
    {
      for (int i = 0; i < repetitions; i++)
      {
        stacks[to - 1] += stacks[from - 1].Last();
        stacks[from - 1] = stacks[from - 1][..^1];
      }  
    }
    else
    {
      drawStacks();
      Console.WriteLine(counter + " - " + line);
      counter++;
      int drawerLength = stacks[from - 1].Length >= repetitions ? repetitions : stacks[from - 1].Length;
      string movingPart = stacks[from - 1][(stacks[from - 1].Length - drawerLength)..];
      if (!string.IsNullOrWhiteSpace(movingPart))
      {
        lastStackNumOfItems = drawerLength;
        lastStackModificationNum = to - 1;
        stacks[to - 1] += movingPart;
        stacks[from - 1] = stacks[from - 1][..^movingPart.Length]; 
      }
    }
  }
}

void drawStacks()
{
  for (int i = 0; i < stacks.Count; i++)
  {
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write(i + 1 + ": ");
    for (int j = 0; j < stacks[i].Length; j++)
    {
      if (lastStackModificationNum == i && j >= stacks[i].Length - lastStackNumOfItems)
      {
        Console.ForegroundColor = ConsoleColor.Red;
      }
      Console.Write(stacks[i][j]);
    }
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("\n");
  }
}
string result = string.Join("", stacks.Select(x => x.Length > 0 ? x.Last().ToString() : " "));

sw.Stop();
Console.WriteLine($"Part one: {result}");
Console.WriteLine($"Part two: {result}");
Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");