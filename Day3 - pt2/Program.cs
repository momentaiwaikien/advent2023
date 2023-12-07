using System.Text.RegularExpressions;

string path = "input.txt";
var locations = new Dictionary<string, int>();
var itemLocs = new Dictionary<string, string>();

string readText = File.ReadAllText(path);
var rows = readText.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
var strings = string.Join('.', rows);
List<string> numbers = string.Join('.', Regex.Split(strings, @"\D|\.+")).Split('.', StringSplitOptions.RemoveEmptyEntries).ToList();

var rownum = 0;
var incrementRowNum = () => rownum += 1;
var getRowNum = () => rownum;
var iter = 0;

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

rows.ForEach(row => { 
  numbers.Distinct().ToList().ForEach(n => {

    var matches = Regex.Matches(row, $"(?<=^|[\\D])+({n})(?=$|[\\D])+");

    foreach(Match found in matches)
    {
      if (found.Success)
      {
        numIndex.Add(iter, int.Parse(n));

        for(var i = 0; i < n.Length; i++)
        {
          var locate = $"{found.Index+i}:{getRowNum()}";
          locations.Add(locate, iter);
        }
        iter++;
      }
    }
  });

  for(int i = 0; i < row.Length; i++)
  {
    if(row[i] == '*')
    {
      itemLocs.Add($"{i}:{getRowNum()}", $"{row[i]}");
    }
  }
  incrementRowNum();
});

//locations.ToList().ForEach(x => Console.WriteLine($"{x.Key}-{x.Value}"));
//numIndex.ToList().ForEach(x => Console.WriteLine($"{x.Key}-{x.Value}"));
//itemLocs.ToList().ForEach(x => Console.WriteLine($"{x.Key}-{x.Value}"));


var returnMatch = (
    KeyValuePair<string,string> itemLoc,
    Dictionary<string, int> locs) 
  => {
  var toSearch = getSearchCoords(itemLoc.Key);
   var found = locs.Where(x => toSearch.Any(s => x.Key == s)).DistinctBy(x => x.Value).ToList();
   //Console.WriteLine($"wtf - {found.Count}");
   return (found.Count == 2) ? numIndex[found.First().Value] * numIndex[found.Last().Value] : 0;
};

 Console.WriteLine(itemLocs.ToList().Sum(l => returnMatch(l, locations)));


Console.WriteLine(matchlist.Sum(x => x.Value));

