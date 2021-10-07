using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace DataService
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 1300);
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
                    Console.WriteLine("Recieved data!");

                    for (int i = 0; i < size; i++)
                    {
                        user_data[i] = Convert.ToChar(data[i]);
                    }

                    string str = new string(user_data);

                    //Console.WriteLine(user_data);

                    string[] finalData = DataProcessor.wordArray(user_data);

                    Database.router(finalData);

                    client.Close();
                });

                childSocketThread.Start();
            }
        }
    }
}
