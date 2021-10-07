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

    }
}
