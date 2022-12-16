using System.Diagnostics;
using Day15;
using Shared;

Stopwatch sw = new();
sw.Start();
FileReader reader = new("input.txt");
(List<Device>? devices, List<Point>? allDevices) = reader.ReadStringLines().ParseDevices();

int yAxisLine = 2_000_000;
// int yAxisLine = 10;
List<Point> lineDevices = allDevices.Where(x => x.Y == yAxisLine).ToList();
int minX = devices.Min(x => x.Beacon.X - x.Distance);
int maxX = devices.Max(x => x.Beacon.X + x.Distance);
for (int x = minX; x <= maxX; x++)
{
  Beacon possibleBeacon = new(x, yAxisLine);

  if (devices.All(x => x.Sensor.CalculateManhattanDistanceTo(possibleBeacon) > x.Distance))
  {
    lineDevices.Add(possibleBeacon);
  }
}

int p1 = maxX - minX + 1 - lineDevices.Count;

sw.Stop();


Console.WriteLine($"Part one: {p1}");
Console.WriteLine($"Part two: {0}");
Console.WriteLine($"Time: {sw.Elapsed}");