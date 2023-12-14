using System.Numerics;
public class SourceMap
{
  public SourceMap(BigInteger dest, BigInteger src, BigInteger diff)
  {
    DestinationMin = dest;
    DestinationMax = dest + diff-1;
    SourceMin = src;
    SourceMax = src + diff-1;
    if (SourceMax < SourceMin)
    {
      throw new Exception ($"{dest}:{src}:{diff}");
    }
  }

  public BigInteger SourceMin {get;set;}
  public BigInteger SourceMax {get;set;}

  public BigInteger SourceDiff => SourceMax - SourceMin + 1;

  public BigInteger DestinationMin {get;set;}
  public BigInteger DestinationMax {get;set;}
  public BigInteger DestinationDiff => DestinationMax - DestinationMin + 1;


  public bool InSource (BigInteger src) 
  {
    return src >= SourceMin && src <= SourceMax;
  }

  public Seed GetSourceMatch(Seed seed)
  {
    BigInteger min = seed.Min >= SourceMin ? seed.Min : SourceMin;
    BigInteger max = seed.Max <= SourceMax ? seed.Max : SourceMax;
    if (SourceMin >= seed.Min && SourceMax <= seed.Max)
    {
      min = SourceMin;
      max = SourceMax;
    }
    var mSeed = Seed.Create(min, max);
    //Console.WriteLine ($"Seed: {seed.Min}:{seed.Max}:{seed.Diff} >> Matched: {mSeed.Min}:{mSeed.Max}:{mSeed.Diff}");
    return mSeed;
  }

  public Seed GetDestination(Seed seed)
  {
    //Console.WriteLine($"Get Destination - {SourceMin}:{SourceMax}:{SourceDiff} == {seed.Min}:{seed.Max}:{seed.Diff}");

    var lowerDiff = seed.Min - SourceMin;
    var max = DestinationMin+lowerDiff+seed.Max-seed.Min;
    
    if (DestinationMax<max) Console.WriteLine("Something has fk'd up");

    var destinationSeed = Seed.Create(DestinationMin+lowerDiff, max);
    //Console.WriteLine($"Destination {destinationSeed.Min}:{destinationSeed.Max}:{destinationSeed.Diff}");
    return destinationSeed;
  }

}