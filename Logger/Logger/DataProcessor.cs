using System.Linq;
using System.Text.RegularExpressions;

namespace Cache
{
    class DataProcessor
    {
        public static string[] wordArray(char[] input)
        {
            string str = new string(input);
            string[] words = Regex.Matches(str, "\\w+").OfType<Match>().Select(m => m.Value).ToArray();
            return words;
        }
    }
}
