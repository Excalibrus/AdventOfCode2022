// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Shared;

Stopwatch sw = new();
sw.Start();

List<Element> elements = new()
{
  new Element {Type = ElementType.Rock, Defeats = ElementType.Scissors, Points = 1, Letters = new List<string> {"A", "X"}},
  new Element {Type = ElementType.Paper, Defeats = ElementType.Rock, Points = 2, Letters = new List<string> {"B", "Y"}},
  new Element {Type = ElementType.Scissors, Defeats = ElementType.Paper, Points = 3, Letters = new List<string> {"C", "Z"}}
};

FileReader reader = new("input.txt");
List<string> lines = reader.ReadStringLines();

List<(Element opponent, Element me)> movesP1 = new();
List<(Element opponent, Element me)> movesP2 = new();
foreach (string line in lines)
{
  List<string> letters = line.Split(" ").ToList();
  if (letters.Count == 2)
  {
    string opponentChar = letters.First();
    string myChar = letters.Last();
    Element opponent = elements.First(x => x.Letters.Contains(opponentChar));
    Element me = elements.First(x => x.Letters.Contains(myChar));
    movesP1.Add((opponent, me));
    
    if (myChar == "X")
    {
      movesP2.Add((opponent, elements.First(x => x.Type == opponent.Defeats)));
    }
    else if (myChar == "Y")
    {
      movesP2.Add((opponent, elements.First(x => x.Type == opponent.Type)));
    }
    else
    {
      movesP2.Add((opponent, elements.First(x => x.Type != opponent.Defeats && x.Type != opponent.Type )));
    }
  }
}

int scoreP1 = calculateScore(movesP1);
int scoreP2 = calculateScore(movesP2);

sw.Stop();

Console.WriteLine($"Score part 1: {scoreP1}");
Console.WriteLine($"Score part 2: {scoreP2}");

Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");

int calculateScore(List<(Element opponent, Element me)> moves)
{
  int score = 0;
  foreach ((Element? opponent, Element? me) in moves)
  {
    if (me.Defeats == opponent.Type) // win
    {
      score += me.Points + 6;
    }
    else if (me.Type == opponent.Type) // draw
    {
      score += me.Points + 3;
    }
    else // loss
    {
      score += me.Points;
    }
  }

  return score;
}
class Element
{
  public ElementType Type { get; set; }
  public ElementType Defeats { get; set; }
  public int Points { get; set; }
  public List<string> Letters { get; set; } = new();
}

enum ElementType
{
  Rock,
  Paper,
  Scissors
}