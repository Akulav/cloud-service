using Microsoft.Data.Sqlite;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LoginService
{
    class Program
    {
        [Obsolete]
        static void Main(string[] args)
        {
                
            IPAddress localhost = IPAddress.Parse("127.0.0.1");
            TcpListener tcpListener = new TcpListener(localhost, 13000);
            tcpListener.Start();
          
            while (true)
            {
                Socket client = tcpListener.AcceptSocket();
                Console.WriteLine("Connection accepted.");
                char[] user_data = new char[999];

                var childSocketThread = new Thread(() =>
                {
                    byte[] data = new byte[100];
                    int size = client.Receive(data);
                    Console.WriteLine("Recieved data: ");

                    for (int i = 0; i < size; i++)
                    {
                        //Console.Write(Convert.ToChar(data[i]));
                        user_data[i] = Convert.ToChar(data[i]);
                    }

                    string str = new string(user_data);

                    Console.WriteLine(user_data);

                    string[] finalData = TextManipulator.wordArray(user_data);

                    Database.router(finalData);

                    client.Close();
                });

                childSocketThread.Start();
            }
         
            
        }
    }
}
