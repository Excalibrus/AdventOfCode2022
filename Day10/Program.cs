using System.Diagnostics;
using Shared;

Stopwatch sw = new();
sw.Start();
FileReader reader = new("input.txt");
List<string> lines = reader.ReadStringLines();

int x = 1;
int currentCycle = 0;
int pixelPosition = 0;

int addxCycles = lines.Count(x => x != "noop") * 2;
int totalCycles = addxCycles + (addxCycles - lines.Count);
List<int> signalStrengths = new();

List<int> checkCycles = Enumerable.Range(0, totalCycles / 40 + 2)
  .Select(n => (n * 40) + 20)
  .ToList();

List<int> checkCrtCycles = Enumerable.Range(0, totalCycles / 40 + 2)
  .Select(n => n * 40)
  .ToList();

List<string> crtLines = new List<string>();
string crtLine = string.Empty;

foreach (string line in lines)
{
  List<int> sprite = new List<int> { x - 1, x, x + 1 };
  if (line.Equals("noop"))
  {
    crtLine += sprite.Contains(pixelPosition) ? "X" : ".";
    currentCycle++;
    pixelPosition++;
    CheckStrength();
  }
  else if (int.TryParse(line.Split(" ").Last(), out int adding))
  {
    crtLine += sprite.Contains(pixelPosition) ? "X" : ".";
    currentCycle++;
    pixelPosition++;
    CheckStrength();
    crtLine += sprite.Contains(pixelPosition) ? "X" : ".";
    currentCycle++;
    pixelPosition++;
    CheckStrength();
    x += adding;
  }
}

void CheckStrength()
{
  if (checkCycles.Contains(currentCycle))
  {
    signalStrengths.Add(currentCycle * x);
  }

  if (checkCrtCycles.Contains(currentCycle))
  {
    crtLines.Add(crtLine);
    crtLine = string.Empty;
    pixelPosition = 0;
  }
}

foreach (string crtLinePrint in crtLines)
{
  Console.WriteLine(crtLinePrint);
}

int p1 = signalStrengths.Sum();


sw.Stop();
Console.WriteLine($"Part one: {p1}");
Console.WriteLine($"Part two: {0}");
Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");