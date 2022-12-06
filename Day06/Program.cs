using System.Diagnostics;
using Shared;

Stopwatch sw = new();
sw.Start();
FileReader reader = new("input.txt");
string input = string.Join("", reader.ReadStringLines());

int p1Characters = 4;
int p2Characters = 14;

int p1 = solve(input, p1Characters);
int p2 = solve(input, p2Characters);


int solve(string input, int characters)
{
  for (int i = characters - 1; i < input.Length; i++)
  {
    List<char> range = input.Skip(i - (characters - 1)).Take(characters).ToList();
    if (range.Count == range.Distinct().Count())
    {
      return i + 1;
    }
  }

  return -1;
}

sw.Stop();
Console.WriteLine($"Part one: {p1}");
Console.WriteLine($"Part two: {p2}");
Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");