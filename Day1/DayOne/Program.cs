
using System.Text.RegularExpressions;

string path = "input.txt";

string readText = File.ReadAllText(path)
.Replace("one","one1one")
.Replace("two","two2two")
.Replace("three","three3three")
.Replace("four","four4four")
.Replace("five","five5five")
.Replace("six","six6six")
.Replace("seven","seven7seven")
.Replace("eight","eight8eight")
.Replace("nine","nine9nine");

var items = readText.Split('\n').ToList().Select(x => x.Trim()).ToList();



Console.WriteLine(items.Sum(x => {
  string numbers = string.Join(String.Empty, Regex.Split(x.Trim(), @"\D+").Where(y => !string.IsNullOrWhiteSpace(y)));

  Console.WriteLine($"base =={numbers}==");

  var val = numbers.Length > 0 
    ? numbers.Length == 1 
      ? $"{numbers.First()}{numbers.First()}" 
      : $"{numbers.First()}{numbers.Last()}"
    : "0"; 

  Console.WriteLine($"guessed {val}");
Console.WriteLine("NEXT");
  return int.Parse(val);
  }));