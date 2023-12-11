using System.Text.RegularExpressions;

string path = "input.txt";
string readText = File.ReadAllText(path);

var IsWithinRange = (long number, long rangeStart, long rangeEnd) =>
{
        return number.CompareTo(rangeStart) >= 0 && number.CompareTo(rangeEnd) <= 0;
};

var texts = Regex.Replace(readText, "([a-zA-Z]| {2}|-)+", " ");

var startTime = DateTime.Now;
Console.WriteLine($"Start: {startTime.ToLongTimeString()}");

var ruleSets = texts.Split(':', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

var stepsRange = new Dictionary<long, List<SourceMap>>();

var SeedsList = new List<long>(ruleSets[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)));

var Seeds = new Dictionary<long, long>();

for(var sd = 0; sd<SeedsList.Count; sd+=2)
{
  Seeds.Add(SeedsList[sd], SeedsList[sd+1]);
}

ruleSets.RemoveAt(0);
ruleSets.RemoveAt(0);
var index = 0;


ruleSets.ForEach(x => {
  var parts = x.Split('\n', StringSplitOptions.RemoveEmptyEntries);
  var rules = parts.Select(i=>i.Split(' ').Select(q => long.Parse(q)).ToList()).ToList();  
  stepsRange.Add(index, rules.Select(rule => new SourceMap(rule[0], rule[1], rule[2])).ToList().ToList());
  index++;
});

var currSeeds = new List<Seed>(Seeds.ToList().OrderBy(x => x.Key).Select(x => new Seed(x.Key, x.Value)));

  for(var i = 0; i < stepsRange.Count; i++)
  {
    Console.WriteLine($"====================CURRENT ITER {i}==================");
    //currSeeds.ForEach(x => Console.WriteLine($"CurrSeeds: {x.Min}:{x.Max}:{x.Diff}"));
    //Console.WriteLine($"CurrDiff: {currSeeds.Sum(x => x.Diff)}");
    var newSeeds = new List<Seed>();
    List<SourceMap> currentMaps = stepsRange[i];
    //currentMaps.ForEach(m => Console.WriteLine($"CurrentMaps: From {m.SourceMin}:{m.SourceMax}:{m.SourceDiff} TO {m.DestinationMin}:{m.DestinationMax}:{m.DestinationDiff}"));
    foreach(var currSeed in currSeeds)
    {
      var matchedSeeds = new List<Seed>();
      var matchedMaps = currentMaps.Where(map => (currSeed.Min >= map.SourceMin && currSeed.Min <= map.SourceMax)||(currSeed.Max <= map.SourceMax && currSeed.Max >= map.SourceMin)).ToList();
        
      matchedMaps.ForEach(matchedMap=> {
        //Console.Write($"## matched map {x.SourceMin}-{x.SourceMax}");
        var matchedSeed = matchedMap.GetSourceMatch(currSeed);
        matchedSeeds.Add(matchedSeed);
      
        //matchedSeeds.ForEach(x => Console.WriteLine($"MatchedSeeds: {x.Min}:{x.Max}:{x.Diff}"));
        Console.WriteLine($"## CurrSeed {currSeed.Min}:{currSeed.Max}:{currSeed.Diff} matched seed {matchedSeed.Min}:{matchedSeed.Max}:{matchedSeed.Diff} - map {matchedMap.SourceMin}:{matchedMap.SourceMax} >> {matchedMap.DestinationMin}:{matchedMap.DestinationMax} ({matchedMap.SourceDiff})");
        var destinationSeed = matchedMap.GetDestination(matchedSeed);
        //Console.WriteLine($"##  DestinationSeed {destinationSeed.Min}-{destinationSeed.Max}");
        newSeeds.Add(destinationSeed);
      });
      //newSeeds.ForEach(x => Console.WriteLine($"AfterMatch: {x.Min}:{x.Max}:{x.Diff}"));
      
      var survivors = currSeed.SplitSeed(matchedSeeds);
      //newSeeds.ForEach(x => Console.WriteLine($"SurvivorSeeds: {x.Min}:{x.Max}:{x.Diff}"));
      matchedSeeds.AddRange(survivors);
      //Console.WriteLine($"currSeed: {currSeed.Min}:{currSeed.Max}:{currSeed.Diff} > Matched {matchedSeeds.Min(x => x.Min)}:{matchedSeeds.Max(x => x.Max)}:{matchedSeeds.Max(x => x.Max)-matchedSeeds.Min(x => x.Min)+1}");
      /*if (currSeed.Min != matchedSeeds.Min(x => x.Min) 
      && currSeed.Max != matchedSeeds.Max(x => x.Max)
      && currSeed.Diff != matchedSeeds.Max(x => x.Max) - matchedSeeds.Min(x => x.Min) + 1 )
      {
        Console.WriteLine("_____________________________Housten it's broke_____________________________");
      }*/
      newSeeds.AddRange(survivors);
    }
    Console.WriteLine($"diffs: {newSeeds.Sum(x => x.Diff)}");
    currSeeds = new List<Seed>(newSeeds.OrderBy(x => x.Min));
    currSeeds.ForEach(x => Console.WriteLine($"newSeeds: {x.Min}:{x.Max}:{x.Diff}"));
  }

var min = currSeeds.OrderBy(x => x.Min).First();
Console.WriteLine($"Min {min.Min}:{min.Max}:{min.Diff}");
var endTime = DateTime.Now;
Console.WriteLine($"End: {endTime.ToLongTimeString()}");
Console.WriteLine($"Elapsed Time: {(endTime - startTime).TotalSeconds}");
