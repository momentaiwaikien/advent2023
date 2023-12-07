
using System.Text.RegularExpressions;

string path = "input.txt";
var validRGB = new Dictionary<string,int>{
    {"red", 12},
    {"green", 13},
    {"blue", 14},
  };

string readText = File.ReadAllText(path);
Console.WriteLine(readText);
var items = readText.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();
Console.WriteLine(items.Count); 

Console.WriteLine(
  items.Sum(g => {
    var gameData = g.Split(':', StringSplitOptions.TrimEntries);
    var gameNumber = int.Parse(gameData.First().Split(' ', StringSplitOptions.TrimEntries).Last());

    char[] splitChar = [',', ';'];

    var data = gameData.Last().Split(splitChar, StringSplitOptions.RemoveEmptyEntries).ToList();
    var round = 0;
    var win = true;
    var rgb = new Dictionary<string,int>{
    {"red", 0},
    {"green", 0},
    {"blue", 0},
  };

    data.ForEach(d => {
      round++;
      var parts = d.Split(' ', StringSplitOptions.RemoveEmptyEntries);
      win = win && int.Parse(parts.First()) <= validRGB[parts.Last()];
      rgb[parts.Last()] = int.Parse(parts.First()) > rgb[parts.Last()] ? int.Parse(parts.First()) : rgb[parts.Last()];
      
      Console.WriteLine($"Game {gameNumber}:{int.Parse(parts.First())} {validRGB[parts.Last()]} === ROUND{round} ({win})- {gameData.Last()}");
    });

Console.WriteLine();
    //return (win) ? gameNumber : 0; 
    return rgb["red"] * rgb["green"] * rgb["blue"];
  })
);