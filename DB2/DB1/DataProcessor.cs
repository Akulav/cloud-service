using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DB1
{
    class DataProcessor
    {
        
        public static string RandomString(int length)
        {
             Random random = new Random();
             const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
             return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string[] wordArray(char[] input)
        {
            string str = new string(input);
            string[] words = Regex.Matches(str, "\\w+").OfType<Match>().Select(m => m.Value).ToArray();
            return words;
        }

    }
}
