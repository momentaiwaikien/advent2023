using System.Text.RegularExpressions;

string path = "input.txt";
string readText = File.ReadAllText(path);
Console.WriteLine(readText);
var rows = readText.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
var dicCards = new Dictionary<int, int>();


var getNumberList = (string inputstring) => {
  var items = Regex.Split(inputstring, @"\D|\.+").Where(x => !string.IsNullOrEmpty(x)).ToList();
  return items.Select(x => int.Parse(x.Trim())).ToList();
};

var getWinners = (string row) =>
{
  var gamePieces = row.Split([':','|']);
  var winningNumbers = getNumberList(gamePieces[1]);
  var playNumbers = getNumberList(gamePieces.Last()).ToDictionary(k => k, v => 1);
  return winningNumbers.Where(x => playNumbers.GetValueOrDefault(x, 0) == 1).Count();
};

for(var i = 0; i < rows.Count; i++){
  dicCards.Add(i, getWinners(rows[i]));
};

var cardsToCheck = new Dictionary<int, int>(dicCards.Select(x => new KeyValuePair<int, int>(x.Key, 1)));

var amt = cardsToCheck.Count;

do
{
  var currCard = cardsToCheck.First();
  var winners = dicCards[currCard.Key];
  var numOfCurrCard = cardsToCheck[currCard.Key];

  if (winners > 0)
  {
    amt += winners * numOfCurrCard;
    var toAdd = Enumerable.Range(currCard.Key+1,winners);
    toAdd.ToList().ForEach(i => cardsToCheck[i] += numOfCurrCard);
  }
  cardsToCheck.Remove(currCard.Key);
} while(cardsToCheck.Keys.Count > 0);

Console.WriteLine(amt);