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
  List<char[]> parts = line.Chunk(line.Length / 2).ToList();
  char[] partOne = parts[0];
  char[] partTwo = parts[1];

  char sameItem = partOne.FirstOrDefault(x => partTwo.Contains(x));
  sum += char.IsUpper(sameItem) ? sameItem - ('A' - upperCaseStarting) : sameItem - ('a' - lowerCaseStarting);
}

int sumP2 = 0;
for (int i = 0; i < lines.Count; i++)
{
  if ((i + 1) % 3 == 0 || i == lines.Count - 1)
  {
    List<char> firstTwoLinesMatches = lines[i - 2].Where(x => lines[i - 1].Contains(x)).ToList();
    char commonMatch = lines[i].FirstOrDefault(x => firstTwoLinesMatches.Contains(x));
    sumP2 += char.IsUpper(commonMatch) ? commonMatch - ('A' - upperCaseStarting) : commonMatch - ('a' - lowerCaseStarting);
  }
  
}

Console.WriteLine($"Part one: {sum}");
Console.WriteLine($"Part two: {sumP2}");
Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");