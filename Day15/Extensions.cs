using Shared;

namespace Day15;

public static class Extensions
{
  public static (List<Device> devices, List<Point> allDevices) ParseDevices(this List<string> lines)
  {
    List<Device> devices = new();
    List<Point> allDevices = new();
    foreach (string line in lines)
    {
      string[] devicesString = line
        .Split(
          new[] { "Sensor at ", ": closest beacon is at " },
          StringSplitOptions.RemoveEmptyEntries);
      string[] sensorString = devicesString[0].Split(",");
      string[] beaconString = devicesString[1].Split(",");
      Beacon beacon = new(
        int.Parse(beaconString[0].Split("=")[1]),
        int.Parse(beaconString[1].Split("=")[1])
      );
      if (allDevices.Contains(beacon))
      {
        beacon.Id = (allDevices.First(x => x == beacon) as Beacon).Id;
      }

      Device device = new()
      {
        Sensor = new Sensor(
          int.Parse(sensorString[0].Split("=")[1]),
          int.Parse(sensorString[1].Split("=")[1])
        ),
        Beacon = beacon
      };
      device.Distance = device.Sensor.CalculateManhattanDistanceTo(device.Beacon);
      allDevices.Add(device.Sensor);
      allDevices.Add(device.Beacon);
      devices.Add(device);
    }

    return (devices, allDevices.Distinct().ToList());
  }

  public static int CalculateManhattanDistanceTo(this Point point, Point toPoint)
  {
    return Math.Abs(point.X - toPoint.X) + Math.Abs(point.Y - toPoint.Y);
  }

  public static List<Point> GetPossiblePointsOnYAxis(this Point specifiedPoint, int yAxis, int distance)
  {
    int yDiff = yAxis - specifiedPoint.Y;

    int y = specifiedPoint.Y + yDiff;

    int x = specifiedPoint.X + (yDiff / 2);

    if (distance % 2 != 0)
    {
      x += (yDiff > 0) ? 1 : -1;
    }

    List<Point> targetPoints = new()
    {
      new Point(x, y)
    };

    if (distance == 0)
    {
      return targetPoints;
    }

    int steps = (distance - (yDiff % 2)) / 2;

    if (distance % 2 != 0 && yDiff % 2 != 0)
    {
      steps += 1;
    }

    for (int i = 1; i <= steps; i++)
    {
      targetPoints.Add(new Point(x + i, y));
    }

    for (int i = 1; i <= steps; i++)
    {
      targetPoints.Add(new Point(x - i, y));
    }

    return targetPoints;
  }
}