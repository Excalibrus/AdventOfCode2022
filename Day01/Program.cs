using System.Diagnostics;
using Shared;

Stopwatch sw = new();
sw.Start();

FileReader reader = new("input.txt");
List<string> lines = reader.ReadStringLines();

List<int> elfs = new();
int currentCalories = 0;
for (int i = 0; i < lines.Count; i++)
{
  if (int.TryParse(lines[i], out int num))
  {
    currentCalories += num;
  }
  if (string.IsNullOrWhiteSpace(lines[i]) || i == lines.Count - 1)
  {
    elfs.Add(currentCalories);
    currentCalories = 0;
  }
}
sw.Stop();
Console.WriteLine($"Part 1: {elfs.Max()}");
Console.WriteLine($"Part 2: {elfs.OrderDescending().Take(3).Sum()}");
Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");