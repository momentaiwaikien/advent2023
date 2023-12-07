using System.Text.RegularExpressions;

var getcoords = (string key) => {
  return key.Split(':').Select(k => int.Parse(k)).ToArray();
};

var getSearchCoords = (int height, int len, int[] locs) => {
  var searchLocs = new List<string>();
  //left
  if (locs[0]-1 > -1) searchLocs.Add($"{locs[0]-1}:{locs[1]}");
  //topleft
  if (locs[0]-1 > -1 && locs[1]-1 > -1) searchLocs.Add($"{locs[0]-1}:{locs[1]-1}");
  //top
  if (locs[1]-1 > -1) searchLocs.Add($"{locs[0]}:{locs[1]-1}");
  //topright
  if (locs[0]+1 < len && locs[1]-1 > -1) searchLocs.Add($"{locs[0]+1}:{locs[1]-1}");
  //right
  if (locs[0]+1 < len) searchLocs.Add($"{locs[0]+1}:{locs[1]}");
  //bottomright
  if (locs[0]+1 < len && locs[1]+1 < height) searchLocs.Add($"{locs[0]+1}:{locs[1]+1}");
  //bottom
  if (locs[1]+1 < height) searchLocs.Add($"{locs[0]}:{locs[1]+1}");
  //bottomleft
  if (locs[0]-1 > -1 && locs[1]+1 < height) searchLocs.Add($"{locs[0]-1}:{locs[1]+1}");
  return searchLocs;
};

var matchlist = new List<KeyValuePair<int, int>>();

var returnMatch = (List<string> searchLocs, Dictionary<string, int> locs) => {
  var matches = searchLocs.Select(x => {
    var v = locs.GetValueOrDefault(x, 0);
    if(v > 0)
    {
      matchlist.Add(new KeyValuePair<int, int>(getcoords(x)[1], v)); 
    }
    //Console.WriteLine($"{x} -- {v}");
    return v;
    }).Distinct();

  return matches;
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

rows.ForEach(row => { 
  numbers.Distinct().ToList().ForEach(n => {

    var found = Regex.Match(row, $"(?<=^|[\\D])+({n})(?=$|[\\D])+");

    if (found.Success)
    {
      //Console.WriteLine($"{getRowNum()}=={row} --- index {found.Index} number {n}");
      for(var i = 0; i < n.Length; i++)
      {
        var locate = $"{getRowNum()}:{found.Index+i}";
        //Console.WriteLine($"Added Location {locate}");
        locations.Add(locate, int.Parse(n));
      }
    }
  });
  Console.WriteLine($"{row}");

  for(int i = 0; i < row.Length; i++)
  {
    if(!Regex.IsMatch($"{row[i]}", @"(\d|\.)+"))
    {
      Console.WriteLine($"item {row[i]} location {getRowNum()}:{i}");
      itemLocs.Add($"{getRowNum()}:{i}", $"{row[i]}");
    }
  }
  incrementRowNum();
});

//locations.ToList().ForEach(x => Console.WriteLine($"{x.Key}-{x.Value}"));
//itemLocs.ToList().ForEach(x => Console.WriteLine($"{x.Key}-{x.Value}"));

 var kk = itemLocs.ToList().SelectMany(l => {
  var loc = getcoords(l.Key);
  var searchLocs = getSearchCoords(rows.Count, rows[loc[0]].Length, loc);
  var matches = returnMatch(searchLocs, locations);
  return matches;
});

Console.WriteLine(kk.Distinct().Sum(x => x));

Console.WriteLine(matchlist.DistinctBy(x => $"{x.Key}:{x.Value}").Sum(x => x.Value));

