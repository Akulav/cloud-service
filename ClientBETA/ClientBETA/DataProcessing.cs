using System.Linq;
using System.Text.RegularExpressions;

namespace ClientBETA
{
    class DataProcessing
    {
        public static string[] GetID(string input)
        {
            string[] textSplit = input.Split();
            return textSplit;
        }

        public static string[] wordArray(char[] input)
        {
            string str = new string(input);
            string[] words = Regex.Matches(str, "\\w+").OfType<Match>().Select(m => m.Value).ToArray();
            return words;
        }
    }
}
