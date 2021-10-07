using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace LoginService
{
    class DataProcessor
    {

        public static void send_response(string data)
        {
            ServerClientSync sc = new ServerClientSync();
            sc.ip = IPAddress.Parse("192.168.10.107");
            sc.send_port = 11000;
            sc.syncWithClient(data);
        }


        
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
