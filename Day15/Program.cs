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

int minLimitX = 0;
int minLimitY = 0;
int maxLimitX = 4_000_000;
int maxLimitY = 4_000_000;
// int maxLimitX = 20;
// int maxLimitY = 20;

List<Device> orderedDevices = devices.OrderByDescending(x => x.Distance).ToList();
List<Beacon> possibleBeacons = new();
for (int y = minLimitY; y <= maxLimitY; y++)
{
  if (y % 10_000 == 0)
  {
    Console.WriteLine(y + "/" + maxLimitY);
  }
  if (possibleBeacons.Count > 0) break;
  for (int x = minLimitX; x <= maxLimitX; x++)
  {
    if (possibleBeacons.Count > 0) break;
    Beacon possibleBeacon = new(x, y);

    if (!orderedDevices.Any(x => x.Sensor.CalculateManhattanDistanceTo(possibleBeacon) <= x.Distance))
    {
      possibleBeacons.Add(possibleBeacon);
    }
  }
}

long p2 = (long)possibleBeacons.First().X * 4_000_000 + possibleBeacons.First().Y;
sw.Stop();


Console.WriteLine($"Part one: {p1}");
Console.WriteLine($"Part two: {p2}");
Console.WriteLine($"Time: {sw.Elapsed}");