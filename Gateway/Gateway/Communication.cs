using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Gateway
{
    class Communication
    {
        public static void send_response(string data, string ip, string port, SqlConnection connection)
        {
            TcpClient tcpClient = new TcpClient(ip, int.Parse(port));
            using (NetworkStream ns = tcpClient.GetStream())
            {
                using (
                    BufferedStream bs = new BufferedStream(ns))
                {
                    byte[] messageBytesToSend = Encoding.UTF8.GetBytes(data);
                    bs.Write(messageBytesToSend, 0, messageBytesToSend.Length);
                }
            }
        }

        public static void send_to_client(string data)
        {

            TcpClient tcpClient = new TcpClient("localHost", 13);
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

        public static void router(string[] data, string data_string)
        {

            if (data[0] == "whitelist")
            {
                if (data[1] == "user")
                {
                    Database.insertData(Database.getDB("user"), data[2], data[3]);
                }
                if (data[1] == "logger")
                {
                    Database.insertData(Database.getDB("logger"), data[2], data[3]);
                }
                if (data[1] == "data")
                {
                    Database.insertData(Database.getDB("data"), data[2], data[3]);
                }
                if (data[1] == "cache")
                {
                    Database.insertData(Database.getDB("cache"), data[2], data[3]);
                }
            }

            else if (data[0] == "healthRply")
            {
                Console.WriteLine("Service: " + data[1] + " is alive.");
            }

            else if (data[0] == "signup")
            {
                Database.processRequest("user", data_string,false);
            }

            else if (data[0] == "login")
            {
                Database.processRequest("user", data_string,false);
            }

            else if (data[0] == "connect" || data[0] == "download" || data[0] == "upload")
            {
                Database.processRequest("data", data_string,false);
            }

            else if (data[0] == "loginNoCache")
            {
                Database.processRequest("cache", data_string,false);
            }

            else
            {
                Database.processRequest("cache", data_string, true);
            }

        }

        public static void listen(int port)
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

                    router(finalData, str);

                    client.Close();
                });

                childSocketThread.Start();
            }
        }

    }
}