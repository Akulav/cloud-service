using System.Linq;
using System.Text.RegularExpressions;

namespace LoginService
{
    class TextManipulator
    {
        public static string[] wordArray(char[] input)
        {
            string str = new string(input);
            string[] words = Regex.Matches(str, "\\w+").OfType<Match>().Select(m => m.Value).ToArray();
            return words;
        }
    }
}
