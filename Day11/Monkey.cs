using System.Numerics;

namespace Day11;

public class Monkey
{
  public int Number { get; set; }
  public List<BigInteger> Items { get; set; } = new();
  public int Operation { get; set; }
  public OperationType OperationType { get; set; }
  public int Divisible { get; set; }
  public long NumberOfInspections { get; set; }
  public Dictionary<bool, int> Answers { get; set; } = new();

  public Monkey(int number)
  {
    Number = number;
  }

  public List<(int monkeyNumber, BigInteger item)> Inspect(int part)
  {
    List<(int monkeyNumber, BigInteger item)> inspectionResults = new();
    foreach (BigInteger item in new List<BigInteger>(Items))
    {
      BigInteger worriedLevel = 0;
      if (OperationType == OperationType.Addition)
      {
        worriedLevel = item + (Operation < 0 ? item : Operation);
      }
      else
      {
        worriedLevel = item * (Operation < 0 ? item : Operation);
      }

      BigInteger worriedLevelAfterBored = part == 1 ? worriedLevel/ 3 : worriedLevel;
      inspectionResults.Add((Answers[worriedLevelAfterBored % Divisible == 0], worriedLevelAfterBored));
      
      Items.Remove(item);
      NumberOfInspections++;
    }

    return inspectionResults;
  }
}