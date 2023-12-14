using System.Numerics;

public class Seed
{
  public Seed()
  {
  }

  public Seed(BigInteger src, BigInteger diff)
  {
    Min = src;
    Max = src + diff-1;
    //Console.WriteLine($"AddSeed {Min}:{Max}:{Diff}");
    
    if (Max < Min)
    {
      throw new Exception ($"{Min}:{Max}:{Diff}");
    }
  }

  public BigInteger Min {get;set;}
  public BigInteger Max {get;set;}

  private BigInteger GetDiff()
  {
    if (Max < Min)
    {
      throw new Exception($"{Min}:{Max}:{Diff}");
    }
    return Max - Min + 1;
  }

  public BigInteger Diff => GetDiff();

  public List<Seed> SplitSeed(List<Seed> maps)
  {
    var returnSeeds = new List<Seed>();

    if (maps.Count == 0)
    {
      returnSeeds.Add(Create(Min, Max));
      return returnSeeds;
    }
    if (maps.Any(x => x.Min == Min && x.Max == Max))
    {
      //Console.WriteLine($"FUll House");
      return returnSeeds;
    }
    
    var orderedMaps = maps.OrderBy(x => x.Min).ToList();

    //Console.WriteLine($"SeedToSplit: {Min}:{Max}:{Diff}");
    //takenSeeds.ForEach(x => Console.WriteLine($"takenSeeds: {x.Min}:{x.Max}:{x.Diff}"));
    //Console.WriteLine($"{takenSeeds.Count} FunneledSeeds");
    //funneledSeeds.ForEach(x => Console.WriteLine($"fs: {x.Min}:{x.Max}:{x.Diff}"));
    var currMin = Min;
    var topMax = Max;
    foreach(var map in orderedMaps)
    {
      if(map.Min > currMin)
      {
        returnSeeds.Add(Create(currMin, map.Min-1));
      }
      currMin = map.Max+1;
    }

    if (currMin < topMax)
    {
      returnSeeds.Add(Create(currMin, Max));
    }
    
    //if (returnSeeds.Count == 0) Console.WriteLine($"XXX NO PARTING DONE XXX");
    //Console.WriteLine($"NumOfParts: {returnSeeds.Count}");
    //returnSeeds.ForEach(r => Console.WriteLine($"Returned Parts: {r.Min}:{r.Max}:{r.Diff}"));
    return returnSeeds;
  }

  public static Seed Create(BigInteger min, BigInteger max)
  {
    return new Seed
    {
      Min = min, Max = max
    };
  }
}