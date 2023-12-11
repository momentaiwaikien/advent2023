public class SourceMap
{
  public SourceMap(long dest, long src, long diff)
  {
    DestinationMin = dest;
    DestinationMax = dest + diff-1;
    SourceMin = src;
    SourceMax = src + diff-1;
  }

  public long SourceMin {get;set;}
  public long SourceMax {get;set;}

  public long SourceDiff => SourceMax - SourceMin + 1;

  public long DestinationMin {get;set;}
  public long DestinationMax {get;set;}
  public long DestinationDiff => DestinationMax - DestinationMin + 1;


  public bool InSource (long src) 
  {
    return src >= SourceMin && src <= SourceMax;
  }

  public Seed GetSourceMatch(Seed seed)
  {
    //79 > 14
    long min = seed.Min >= SourceMin ? seed.Min : SourceMin;
    long max = seed.Max <= SourceMax ? seed.Max : SourceMax;
    var mSeed = Seed.Create(min, max);
    //Console.WriteLine ($"Seed: {seed.Min}:{seed.Max}:{seed.Diff} >> Matched: {mSeed.Min}:{mSeed.Max}:{mSeed.Diff}");
    return mSeed;
  }

  public Seed GetDestination(Seed seed)
  {
    //Console.WriteLine($"Get Destination - {SourceMin}:{SourceMax}:{SourceDiff} == {seed.Min}:{seed.Max}:{seed.Diff}");

    var minDiff = seed.Min - SourceMin;
    var max = DestinationMin+minDiff+seed.Diff-1;
    
    if (DestinationMax<max) Console.WriteLine("Something has fk'd up");

    var destinationSeed = Seed.Create(DestinationMin+minDiff, max);
    //Console.WriteLine($"Destination {destinationSeed.Min}:{destinationSeed.Max}:{destinationSeed.Diff}");
    return destinationSeed;
  }

}