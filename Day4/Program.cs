using System.Text.RegularExpressions;

string path = "input.txt";
string readText = File.ReadAllText(path);
Console.WriteLine(readText);
var rows = readText.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

var getNumberList = (string inputstring) => {
  var items = Regex.Split(inputstring, @"\D|\.+").Where(x => !string.IsNullOrEmpty(x)).ToList();
  return items.Select(x => int.Parse(x.Trim())).ToList();
};

Console.WriteLine(rows.Sum(row => { 
  var gamePieces = row.Split([':','|']);
  var winningNumbers = getNumberList(gamePieces[1]);
  var playNumbers = getNumberList(gamePieces.Last()).ToDictionary(k => k, v => 1);
  var numOfWinners = winningNumbers.Where(x => playNumbers.GetValueOrDefault(x, 0) == 1).Count();
  return numOfWinners > 0 ? Math.Pow(2,numOfWinners-1) : 0;
}));
