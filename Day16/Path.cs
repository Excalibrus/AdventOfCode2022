namespace Day16;

public class Path
{
  public string From { get; set; }

  public string Destination { get; set; }

  public List<List<Valve>> AllPaths { get; set; } = new();
}