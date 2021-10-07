using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace LoginService
{
    class DataProcessor
    {

        public static void send_response(string data, string ip, int port)
        {
            TcpClient tcpClient = new TcpClient(ip, port);
            using (NetworkStream ns = tcpClient.GetStream())
            {

                using (
                    BufferedStream bs = new BufferedStream(ns))
                {
                    byte[] messageBytesToSend = Encoding.UTF8.GetBytes(data);
                    bs.Write(messageBytesToSend, 0, messageBytesToSend.Length);
                }

            }
            tcpClient.Close();

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
