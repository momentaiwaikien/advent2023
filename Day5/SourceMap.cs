public class SourceMap
{
  public SourceMap(long dest, long src, long diff)
  {
    DestinationMin = dest;
    SourceMin = src;
    SourceMax = src + diff-1;
    Diff = diff;
  }

  public long SourceMin {get;set;}
  public long SourceMax {get;set;}

  public long DestinationMin {get;set;}

  public long Diff {get;set;}

  public bool InSource (long src) 
  {
    return src >= SourceMin && src <= SourceMax;
  }

  public long GetDestination(long src)
  {
    var diff = src-SourceMin;
    return DestinationMin+diff;
  }

}