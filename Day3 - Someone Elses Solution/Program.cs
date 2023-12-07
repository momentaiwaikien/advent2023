using System.Text.RegularExpressions;

char[] Symbols = { '@', '#', '$', '%', '&', '*', '/', '+', '-', '=' };
string pattern = @"\d+";
List<string>? list;
list = new List<string>((await File.ReadAllLinesAsync(@"Input.txt")));

var AnyIndexInList = (List<int> list, int startIndex, int length) =>
{
    for (int i = startIndex; i < startIndex + length; i++)
    {
        if (list.Contains(i))
        {
            return true;
        }
    }
    return false;
};

var Calculate = (string line, int col) =>
{
    List<int> indexesToCheck = new List<int> { col - 1, col, col + 1 };
    int count = 0;
    MatchCollection matches = Regex.Matches(line, pattern);

    foreach (Match match in matches)
    {
        string number = match.Value;

        if (AnyIndexInList(indexesToCheck, match.Index, match.Length))
        {
            count += Int32.Parse(number);
        }
    }
    return count;
};

int count = 0;
for (int row = 0; row < list.Count; row++)
{
    for (int col = 0; col < list[row].Length; col++)
    {
        var c = list[row][col];
        if (c == '.')
        {
            continue;
        }

        if (Symbols.Contains(c))
        {
            var res = Calculate(list[row - 1], col);
            res += Calculate(list[row], col);
            res += Calculate(list[row + 1], col);
            count += res;
        }

    }
}
Console.WriteLine(count);
