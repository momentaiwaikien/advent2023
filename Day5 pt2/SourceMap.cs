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

  public long DestinationMin {get;set;}
  public long DestinationMax {get;set;}

  public bool InSource (long src) 
  {
    return src >= SourceMin && src <= SourceMax;
  }

  public Seed GetSourceMatch(Seed seed)
  {
    //79 > 14
    long min = seed.Min >= SourceMin ? seed.Min : SourceMin;
    long max = seed.Max <= SourceMax ? seed.Max : SourceMax;
    return Seed.Create(min, max);
  }

  public Seed GetDestination(Seed seed)
  {
    var minDiff = seed.Min - SourceMin;
    return Seed.Create(DestinationMin+minDiff, DestinationMin+minDiff+seed.Diff);
  }

}