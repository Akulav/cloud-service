using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Gateway
{
    class Communication
    {
        public static void Send_response(string data, string ip, string port)
        {
            TcpClient tcpClient = new TcpClient(ip, int.Parse(port));
            using NetworkStream ns = tcpClient.GetStream();
            using BufferedStream bs = new BufferedStream(ns);
            byte[] messageBytesToSend = Encoding.UTF8.GetBytes(data);
            bs.Write(messageBytesToSend, 0, messageBytesToSend.Length);
        }

        public static void Router(string[] data, string data_string)
        {

            if (data[0] == "whitelist")
            {
                if (data[1] == "user")
                {
                    Database.InsertData(Database.GetDB("user"), data[2], data[3]);
                }
                else if (data[1] == "logger")
                {
                    Database.InsertData(Database.GetDB("logger"), data[2], data[3]);
                }
                else if (data[1] == "data")
                {
                    Database.InsertData(Database.GetDB("data"), data[2], data[3]);
                }
                else if (data[1] == "cache")
                {
                    Database.InsertData(Database.GetDB("cache"), data[2], data[3]);
                }
            }

            else if (data[0] == "signup")
            {
                Database.ProcessRequest("user", data_string, false);
            }

            else if (data[0] == "login")
            {
                Database.ProcessRequest("cache", data_string, false);
            }

            else if (data[0] == "connect" || data[0] == "download" || data[0] == "upload")
            {
                Database.ProcessRequest("data", data_string, false);
            }

            else if (data[0] == "loginNoCache")
            {
                Database.ProcessRequest("user", data_string, false);
            }

            else
            {
                Console.WriteLine("sending to client...");
                Database.ProcessRequest("cache", data_string, true);
                Console.WriteLine("SENDING TO CACHE: " + data_string);
            }

        }
       
        public static void Listen(int port)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();

            Console.WriteLine("GATEWAY INITIALIZED...");

            while (true)
            {

                Socket client = tcpListener.AcceptSocket();
                Console.WriteLine("Connection accepted.");
                char[] user_data = new char[9999];

                var childSocketThread = new Thread(() =>
                {
                    byte[] data = new byte[1000];
                    int size = client.Receive(data);
                    Console.WriteLine("Recieved data!");

                    for (int i = 0; i < size; i++)
                    {
                        user_data[i] = Convert.ToChar(data[i]);
                    }

                    string str = new string(user_data);
                    string[] finalData = DataProcessor.wordArray(user_data);
                    Console.WriteLine("Data=" + str);
                    Router(finalData, str);

                    client.Close();
                });

                childSocketThread.Start();
            }
        }

    }
}