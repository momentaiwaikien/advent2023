using System.Text.RegularExpressions;

string path = "input.txt";
string readText = File.ReadAllText(path);

var texts = Regex.Replace(readText, "([a-zA-Z]| {2}|-)+", " ");

var ruleSets = texts.Split(':', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

var stepsRange = new Dictionary<long, List<SourceMap>>();

var seeds = new List<long>(ruleSets[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)));

ruleSets.RemoveAt(0);
ruleSets.RemoveAt(0);
var index = 0;


ruleSets.ForEach(x => {
  var parts = x.Split('\n', StringSplitOptions.RemoveEmptyEntries);
  var rules = parts.Select(i=>i.Split(' ').Select(q => long.Parse(q)).ToList()).ToList();  
  stepsRange.Add(index, rules.Select(rule => new SourceMap(rule[0], rule[1], rule[2])).ToList());
  index++;
});

var min = seeds.Min(seed => {
  var currSeed = seed;
  Console.Write($"{currSeed}");
  for(var mindex = 0; mindex < stepsRange.Count; mindex++)
  {
    Console.Write($"-{currSeed}");
    currSeed = stepsRange[mindex]
      .FirstOrDefault(map => map.InSource(currSeed))
      ?.GetDestination(currSeed) 
    ?? currSeed;
  }
  Console.WriteLine();
  return currSeed;
});

Console.WriteLine(min);