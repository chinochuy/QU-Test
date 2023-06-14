
#region First try
///In this first attempt, I tried to use more linq and not for loops, but honestly my mind went blank and I'm a little rusty using it hehe.
///I only managed to get the words horizontally by using reverse to search for words in both directions.


//using System.Linq;
//using System.Text.RegularExpressions;

//IEnumerable<string> _matix = new List<string>() { "abcec", "fgwio", "chill", "pqnsd", "uvdxy" };

//List<string> _findWords = new List<string>() { "chill", "snow", "cold", "wind" };
//Find2(_findWords);

//IEnumerable<string> Find2(IEnumerable<string> wordstream)
//{
//    List<string> result = new List<string>();
//    if (wordstream != null && wordstream.Count() > 0)
//    {
//        //Create dictionary from wordstream
//        Dictionary<string, int> dic = wordstream.Where(x => !string.IsNullOrEmpty(x)).GroupBy(x => x).ToDictionary(x => x.Key, x => 0);

//        //foreach in matrix list
//        foreach (string word in _matix)
//        {
//            //Read words horizontally 
//            foreach (string word2 in dic.Keys)
//            {
//                var regex = new Regex(word2);

//                if (regex.IsMatch(word)) 
//                {
//                    int countMatch = regex.Matches(word).Count();
//                    dic[word2] += countMatch;
//                }

//                char[] reverseWord = word.ToCharArray();
//                Array.Reverse(reverseWord);
//                string toReverseWord = new string(reverseWord);

//                if (regex.IsMatch(toReverseWord))
//                {
//                    int countMatch = regex.Matches(toReverseWord).Count();
//                    dic[word2] += countMatch;
//                }
//            }
//        }

//        result = dic.OrderBy(x => x.Value).Take(10).Select(x => x.Key).ToList();

//        foreach (string item in result) 
//        {
//            Console.WriteLine(item);
//            Console.WriteLine(dic[item]);
//        }
//    }

//    return result;
//}
#endregion

#region Second try
///This second attempt is the most used for loops, in something which is very common to use, I don't think it has the best performance but I think it meets the result.

#region Call Methods
char[,] _matrix;
var matrix = new List<string>
{
    "abcec",
    "fgwio",
    "chill",
    "pqnsd",
    "uvdxy"
};

WordFinderMethod(matrix);
var wordStream = new List<string> { "chill", "cold", "wind", "snow" };
var foundWords = Find(wordStream);

foreach (var word in foundWords)
{
    Console.WriteLine(word);
}
#endregion

///WordFinderMethod create matrix
void WordFinderMethod(IEnumerable<string> matrix)
{
    if (matrix == null)
        throw new ArgumentNullException(nameof(matrix));

    var rowCount = matrix.Count();
    var columnCount = matrix.First().Length;
    _matrix = new char[rowCount, columnCount];

    var rowIndex = 0;
    foreach (var row in matrix)
    {
        if (row.Length != columnCount)
            throw new ArgumentException("All rows in the matrix must have the same length.");

        for (var columnIndex = 0; columnIndex < columnCount; columnIndex++)
        {
            _matrix[rowIndex, columnIndex] = row[columnIndex];
        }

        rowIndex++;
    }
}

///Find method send word you need to search into matrix
IEnumerable<string> Find(IEnumerable<string> wordstream)
{
    if (wordstream == null)
        throw new ArgumentNullException(nameof(wordstream));

    var wordCount = new Dictionary<string, int>();
    var rowCount = _matrix.GetLength(0);
    var columnCount = _matrix.GetLength(1);

    foreach (var word in wordstream)
    {
        var wordOccurrences = new HashSet<(int, int)>();

        // Search horizontally
        for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
        {
            for (var columnIndex = 0; columnIndex <= columnCount - word.Length; columnIndex++)
            {
                if (CheckWordMatch(word, rowIndex, columnIndex, 0, 1, wordOccurrences))
                {
                    wordCount[word] = wordCount.GetValueOrDefault(word) + 1;
                    break;
                }
            }
        }

        // Search vertically
        for (var rowIndex = 0; rowIndex <= rowCount - word.Length; rowIndex++)
        {
            for (var columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                if (CheckWordMatch(word, rowIndex, columnIndex, 1, 0, wordOccurrences))
                {
                    wordCount[word] = wordCount.GetValueOrDefault(word) + 1;
                    break;
                }
            }
        }
    }

    return wordCount.OrderByDescending(kv => kv.Value).Select(kv => kv.Key).Take(10);
}

///CheckWordMatch method to check if word exist into matrix
bool CheckWordMatch(string word, int startRow, int startColumn, int rowStep, int columnStep, HashSet<(int, int)> occurrences)
{
    for (var i = 0; i < word.Length; i++)
    {
        var row = startRow + i * rowStep;
        var column = startColumn + i * columnStep;

        if (_matrix[row, column] != word[i] || occurrences.Contains((row, column)))
            return false;

        occurrences.Add((row, column));
    }

    return true;
}

#endregion
