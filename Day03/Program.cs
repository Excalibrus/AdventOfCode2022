using System.Diagnostics;
using System.Linq.Expressions;
using Shared;

Stopwatch sw = new();
sw.Start();

int lowerCaseStarting = 1;
int upperCaseStarting = 27;
FileReader reader = new("input.txt");
List<string> lines = reader.ReadStringLines();

int sum = 0;

foreach (string line in lines)
{
  char[] partOne = line.Chunk(line.Length / 2).First();
  char[] partTwo = line.Chunk(line.Length / 2).Last();

  char sameItem = partOne.FirstOrDefault(x => partTwo.Contains(x));
  sum += CalculateSum(sameItem);
}

int sumP2 = 0;
foreach (string[] chunks in lines.Chunk(3))
{
  List<char> firstTwoLinesMatches = chunks[0].Where(x => chunks[1].Contains(x)).ToList();
  char commonMatch = chunks[2].FirstOrDefault(x => firstTwoLinesMatches.Contains(x));
  sumP2 += CalculateSum(commonMatch);
}

int CalculateSum(char c)
{
  return char.IsUpper(c) ? c - ('A' - upperCaseStarting) : c - ('a' - lowerCaseStarting);
}

Console.WriteLine($"Part one: {sum}");
Console.WriteLine($"Part two: {sumP2}");
Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");