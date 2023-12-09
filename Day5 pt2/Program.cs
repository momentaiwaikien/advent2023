using System.Text.RegularExpressions;

string path = "input.txt";
string readText = File.ReadAllText(path);

var IsWithinRange = (long number, long rangeStart, long rangeEnd) =>
{
        return number.CompareTo(rangeStart) >= 0 && number.CompareTo(rangeEnd) <= 0;
};

var texts = Regex.Replace(readText, "([a-zA-Z]| {2}|-)+", " ");

var ruleSets = texts.Split(':', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

var stepsRange = new Dictionary<long, List<SourceMap>>();

var SeedsList = new List<long>(ruleSets[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)));

var Seeds = new Dictionary<long, long>();

for(var sd = 0; sd<SeedsList.Count; sd+=2)
//for(var sd = 0; sd<2; sd+=2)
{
  Seeds.Add(SeedsList[sd], SeedsList[sd+1]);
}

ruleSets.RemoveAt(0);
ruleSets.RemoveAt(0);
var index = 0;


ruleSets.ForEach(x => {
  var parts = x.Split('\n', StringSplitOptions.RemoveEmptyEntries);
  var rules = parts.Select(i=>i.Split(' ').Select(q => long.Parse(q)).ToList()).ToList();  
  stepsRange.Add(index, rules.Select(rule => new SourceMap(rule[0], rule[1], rule[2])).ToList().OrderBy(o => o.SourceMin).ToList());
  index++;
});

var currSeeds = new List<Seed>(Seeds.ToList().OrderBy(x => x.Key).Select(x => new Seed(x.Key, x.Value)));

  for(var i = 0; i < stepsRange.Count; i++)
  {
    var newSeeds = new List<Seed>();
    List<SourceMap> currentMaps = stepsRange[i];
    Console.WriteLine(i);
    foreach(var currSeed in currSeeds)
    {
      List<Seed> matchedSeeds = new List<Seed>();
      var matchedmaps = currentMaps.Where(x => (currSeed.Min >= x.SourceMin && currSeed.Min <= x.SourceMax)||(currSeed.Max <= x.SourceMax && currSeed.Max >= x.SourceMin)).ToList();
        
      matchedmaps.ForEach(x=> {
        //Console.Write($"## matched map {x.SourceMin}-{x.SourceMax}");
        var matchedSeed = x.GetSourceMatch(currSeed);
        matchedSeeds.Add(matchedSeed);
        //Console.Write($"## matched seed {matchedSeed.Min}-{matchedSeed.Max}");
        var destinationSeed = x.GetDestination(matchedSeed);
        //Console.WriteLine($"##  DestinationSeed {destinationSeed.Min}-{destinationSeed.Max}");
        newSeeds.Add(destinationSeed);
      });

      var survivors = currSeed.SplitSeed(matchedSeeds);
      survivors.ForEach(x => Console.WriteLine($"survivors {x.Min}-{x.Max}"));
      newSeeds.AddRange(survivors);
    }
    Console.WriteLine("");
    newSeeds.ForEach(x => Console.WriteLine($"{x.Min}:{x.Max}"));
    currSeeds = new List<Seed>(newSeeds);
  }

var min = currSeeds.Min(x => x.Min);
Console.WriteLine(min);