using System.Text.RegularExpressions;

var getcoords = (string key) => {
  return key.Split(':').Select(k => int.Parse(k)).ToArray();
};

var getSearchCoords = (string key) => {
  var locs = getcoords(key);
  var x = locs[0];
  var y = locs[1];
  return new List<string>
  {
    //left
    $"{x - 1}:{y}",
    //topleft
    $"{x - 1}:{y - 1}",
    //top
    $"{x}:{y - 1}",
    //topright
    $"{x + 1}:{y - 1}",
    //right
    $"{x + 1}:{y}",
    //bottomright
    $"{x + 1}:{y + 1}",
    //bottom
    $"{x}:{y + 1}",
    //bottomleft
    $"{x - 1}:{y + 1}"
  };
};

var matchlist = new List<KeyValuePair<int, int>>();
var numIndex = new Dictionary<int, int>();

var returnMatch = (
    KeyValuePair<string,int> searchLoc,
    Dictionary<string, string> ilocs) 
  => {
  var toSearch = getSearchCoords(searchLoc.Key);
   toSearch.ForEach(x => {
    var v ="";
    if(ilocs.TryGetValue(x, out v))
    {
      matchlist.Add(new KeyValuePair<int, int>(searchLoc.Value, numIndex[searchLoc.Value])); 
    }
    
    });
};

//Console.WriteLine();

string path = "input.txt";
var locations = new Dictionary<string, int>();
var itemLocs = new Dictionary<string, string>();

string readText = File.ReadAllText(path);
Console.WriteLine(readText);
var rows = readText.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
var strings = string.Join('.', rows);
List<string> numbers = string.Join('.', Regex.Split(strings, @"\D|\.+")).Split('.', StringSplitOptions.RemoveEmptyEntries).ToList();

var rownum = 0;
var incrementRowNum = () => rownum += 1;
var getRowNum = () => rownum;
var iter = 0;

rows.ForEach(row => { 
  numbers.Distinct().ToList().ForEach(n => {

    var matches = Regex.Matches(row, $"(?<=^|[\\D])+({n})(?=$|[\\D])+");

    foreach(Match found in matches)
    {
      if (found.Success)
      {
        numIndex.Add(iter, int.Parse(n));

        //Console.WriteLine($"{getRowNum()}=={row} --- index {found.Index} number {n}");
        for(var i = 0; i < n.Length; i++)
        {
          var locate = $"{found.Index+i}:{getRowNum()}";
          //Console.WriteLine($"Added Location {locate}");
          locations.Add(locate, iter);
        }
        iter++;
      }
    }
  });

  for(int i = 0; i < row.Length; i++)
  {
    if(!Regex.IsMatch($"{row[i]}", @"(\d|\.)+"))
    {
      //Console.WriteLine($"item {row[i]} location {i}:{getRowNum()}");
      itemLocs.Add($"{i}:{getRowNum()}", $"{row[i]}");
    }
  }
  incrementRowNum();
});

//locations.ToList().ForEach(x => Console.WriteLine($"{x.Key}-{x.Value}"));
//numIndex.ToList().ForEach(x => Console.WriteLine($"{x.Key}-{x.Value}"));
//itemLocs.ToList().ForEach(x => Console.WriteLine($"{x.Key}-{x.Value}"));

 locations.ToList().ForEach(l => {
  returnMatch(l, itemLocs);
});


Console.WriteLine(matchlist.DistinctBy(x => x.Key).Sum(x => x.Value));

//Draw it for diffchecking
/*var abc = locations.Select(x => getcoords(x.Key));

var def = itemLocs.Select(x => getcoords(x.Key));

var things = new List<string>();
things.AddRange(abc.Select(x => $"{x[1]}:{x[0]}"));
things.AddRange(def.Select(x => $"{x[1]}:{x[0]}"));

var abcMaxX = abc.Max(x => x[0]);
var defMaxX = def.Max(x => x[0]);
var abcMaxY = abc.Max(x => x[1]);
var defMaxY = def.Max(x => x[1]);

var maxX = abcMaxX > defMaxX ? abcMaxX : defMaxX;
var maxY = abcMaxY > defMaxY ? abcMaxY : defMaxY;

for(var y=0; y <= maxY; y++)
{
  var line = "";
  for(var x=0; x <= maxX; x++)
  {
    line+=things.Any(i => i == $"{y}:{x}")? "X" : ".";
  }
  Console.WriteLine(line);
}*/

