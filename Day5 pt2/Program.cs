using System.Text.RegularExpressions;

Console.WriteLine($"start:{DateTime.Now.ToLongTimeString()}");

string path = "input.txt";
string readText = File.ReadAllText(path);

var texts = Regex.Replace(readText, "([a-zA-Z]| {2}|-)+", " ");

var ruleSets = texts.Split(':', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

var stepsRange = new Dictionary<long, List<SourceMap>>();

var seedsList = new List<long>(ruleSets[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)));

var seeds = new Dictionary<long, long>();

for(var sd = 0; sd<seedsList.Count; sd+=2)
{
  seeds.Add(seedsList[sd], seedsList[sd+1]);
}

ruleSets.RemoveAt(0);
ruleSets.RemoveAt(0);
var index = 0;


ruleSets.ForEach(x => {
  var parts = x.Split('\n', StringSplitOptions.RemoveEmptyEntries);
  var rules = parts.Select(i=>i.Split(' ').Select(q => long.Parse(q)).ToList()).ToList();  
  stepsRange.Add(index, rules.Select(rule => new SourceMap(rule[0], rule[1], rule[2])).ToList());
  index++;
});

long min = 999999999999999999;

seeds.ToList().ForEach(seed => {
  var longList = new List<long>();
  for(var i = 0; i < seed.Value; i++)
  {
    var currSeed = seed.Key+i;
    //Console.Write($"{currSeed}");
    for(var mindex = 0; mindex < stepsRange.Count; mindex++)
    {
      //Console.Write($"-{currSeed}");
      currSeed = stepsRange[mindex]
        .FirstOrDefault(map => map.InSource(currSeed))
        ?.GetDestination(currSeed) 
      ?? currSeed;
    }
    //Console.WriteLine();
    min = currSeed < min ? currSeed : min;
  }
});

Console.WriteLine($"start:{DateTime.Now.ToLongTimeString()}");
Console.WriteLine(min);