public class Seed
{
  public Seed()
  {
  }

  public Seed(long src, long diff)
  {
    Min = src;
    Max = src + diff-1;
  }

  public long Min {get;set;}
  public long Max {get;set;}

  public long Diff => Max-Min;

  public List<Seed> SplitSeed(List<Seed> takenSeeds)
  {
    var returnSeeds = new List<Seed>();

    if (takenSeeds.Count == 0)
      returnSeeds.Add(Seed.Create(Min, Max));

    var currMin = Min;
    var topMax = Max;
    var index = 1;
    foreach(var takenSeed in takenSeeds)
    {
      //Console.WriteLine($"{index}: {currMin}:{topMax} === ");
      if(index == takenSeeds.Count)
      {
        if (takenSeed.Max + 1 < topMax)
        {
          returnSeeds.Add(Seed.Create(takenSeed.Max +1 , topMax));
        }
      }
      if(takenSeed.Min > currMin)
      {
        returnSeeds.Add(Seed.Create(currMin, takenSeed.Min-1));
      }
      currMin = takenSeed.Max+1;
      index++;
    }
    return returnSeeds;
  }

  public static Seed Create(long min, long max)
  {
    return new Seed
    {
      Min = min, Max = max
    };
  }
}