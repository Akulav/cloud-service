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
        public static void send_to_user(string data)
        {

            TcpClient tcpClient = new TcpClient("localHost", 13000);
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

        public static void send_to_data(string data)
        {

            TcpClient tcpClient = new TcpClient("localHost", 1300);
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


        public static void router(string[] data, string data_string, SqlConnection connection)
        {


            if (data[0]=="signup")
            {
                send_to_user(data_string);          
            }

            else if (data[0] == "login")
            {
                string reply = Database.checkHash(connection, data[3]);

                if (reply == null)
                {
                    Database.insertCacheHash(connection, data[3]);
                    send_to_user(data_string);
                }

                else
                {
                    send_to_client(reply);
                }
            }

            else if (data[0] == "connect" || data[0] == "upload" || data[0] == "download")
            {
                send_to_data(data_string);
            }

            else
            {
                send_to_client(data_string);
                Database.insertCacheReply(connection, data_string, data[data.Length-1]);
            }
        }

        public static void listen(int port, SqlConnection connection)
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
                    Console.WriteLine("Data="+str);
                    Communication.router(finalData, str, connection);

                    client.Close();
                });

                childSocketThread.Start();
            }
        }

    }
}
