namespace Day11;

public class Monkey
{
  public int Number { get; set; }
  public List<int> Items { get; set; } = new();
  public int Operation { get; set; }
  public OperationType OperationType { get; set; }
  public int Divisible { get; set; }
  public int NumberOfInspections { get; set; }
  public Dictionary<bool, int> Answers { get; set; } = new();

  public Monkey(int number)
  {
    Number = number;
  }

  public List<(int monkeyNumber, int item)> Inspect()
  {
    List<(int monkeyNumber, int item)> inspectionResults = new();
    foreach (int item in new List<int>(Items))
    {
      int worriedLevel = 0;
      if (OperationType == OperationType.Addition)
      {
        worriedLevel = item + (Operation < 0 ? item : Operation);
      }
      else
      {
        worriedLevel = item * (Operation < 0 ? item : Operation);
      }

      int worriedLevelAfterBored = worriedLevel / 3;
      inspectionResults.Add((Answers[worriedLevelAfterBored % Divisible == 0], worriedLevelAfterBored));
      
      Items.Remove(item);
      NumberOfInspections++;
    }

    return inspectionResults;
  }
}