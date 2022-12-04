using System.Diagnostics;
using Shared;

Stopwatch sw = new();
sw.Start();
FileReader reader = new("input.txt");
List<string> lines = reader.ReadStringLines();

int counterP1 = 0;
int counterP2 = 0;
foreach (string line in lines)
{
  int leftLow = int.Parse(line.Split(",")[0].Split("-")[0]);
  int leftHigh = int.Parse(line.Split(",")[0].Split("-")[1]);
  int rightLow = int.Parse(line.Split(",")[1].Split("-")[0]);
  int rightHigh = int.Parse(line.Split(",")[1].Split("-")[1]);

  if (leftLow >= rightLow && leftHigh <= rightHigh ||
      rightLow >= leftLow && rightHigh <= leftHigh)
  {
    counterP1++;
  }

  if (leftLow >= rightLow && leftLow <= rightHigh ||
      leftHigh >= rightLow && leftHigh <= rightHigh || 
      rightLow >= leftLow && rightLow <= leftHigh ||
      rightHigh >= leftLow && rightHigh <= leftHigh)
  {
    counterP2++;
  }
}

sw.Stop();
Console.WriteLine($"Part one: {counterP1}");
Console.WriteLine($"Part two: {counterP2}");
Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");