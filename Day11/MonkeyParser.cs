namespace Day11;

public static class MonkeyParser
{
  public static List<Monkey> ParseMonkeys(this List<string> lines)
  {
    List<Monkey> monkeys = new();
    Monkey monkey = null;

    foreach (string line in lines)
    {
      if (string.IsNullOrWhiteSpace(line))
      {
        monkeys.Add(monkey);
      }

      if (line.Contains("Monkey"))
      {
        if (int.TryParse(line.Split('y', ':')[1], out int monkeyNumber))
        {
          monkey = new Monkey(monkeyNumber);
        }
      }
      else if (line.Contains("Starting items:"))
      {
        monkey.Items = line.Split(":")[1].Split(",").Select(int.Parse).ToList();
      }
      else if (line.Contains("Operation"))
      {
        string[] split = line.Split(" = ")[1].Split(" ");
        monkey.Operation += split[2] == "old" ? -1 : int.Parse(split[2]);
        monkey.OperationType = split[1] == "+" ? OperationType.Addition : OperationType.Multiplication;
      }
      else if (line.Contains("Test"))
      {
        monkey.Divisible = int.Parse(line.Split("by")[1]);
      }
      else if (line.Contains("If true"))
      {
        monkey.Answers.Add(true, int.Parse(line.Split("monkey")[1]));
      }
      else if (line.Contains("If false"))
      {
        monkey.Answers.Add(false, int.Parse(line.Split("monkey")[1]));
      }
    }

    monkeys.Add(monkey);
    return monkeys;
  }
}