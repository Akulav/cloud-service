using Community.CsharpSqlite.SQLiteClient;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DataService
{
    class Communications
    {

        public static void send_data(string ip, int port, string command, string arg1)
        {
            TcpClient tcpClient = new TcpClient(ip, port);
            using (NetworkStream ns = tcpClient.GetStream())
            {

                using (
                    BufferedStream bs = new BufferedStream(ns))
                {
                    byte[] messageBytesToSend = Encoding.UTF8.GetBytes(command + " " + arg1);
                    bs.Write(messageBytesToSend, 0, messageBytesToSend.Length);
                }

            }
        }


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

        public static void listen(int port, SqliteConnection connection)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();

            Console.WriteLine("DATASERVICE INITIALIZED...");

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
                    Console.WriteLine("DATA=" + str);
                    Database.router(finalData, connection);

                    client.Close();
                });

                childSocketThread.Start();
            }
        }

    }
}
