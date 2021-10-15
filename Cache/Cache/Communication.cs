using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Cache
{
    class Communication
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



        public static void router(string[] data, string data_string, SqlConnection connection)
        {



            if (data[0] == "login")
            {

                string reply = Database.checkHash(connection, data[3]);
                if (reply == null)
                {
                    Database.insertCacheHash(connection, data[3]);
                    data[0] = "loginNoCache";
                    string converted = String.Join(" ", data);
                    send_response(converted, "localhost", 130);
                    Console.WriteLine("converted: " + converted);
                }

                else
                {
                    Console.WriteLine("HERE");
                    send_response(reply, "localhost", 13);
                }
            }

            else if (data[0] == "health")
            {
                send_response("healthRply cache", "localhost", 130);
            }

            else
            {
                Console.WriteLine("TO BE CACHED:" + data_string);
                Database.insertCacheReply(connection, data_string, data[data.Length - 1]);
            }

        }

        public static void listen(int port, SqlConnection connection)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();

            Console.WriteLine("CACHE INITIALIZED...");

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

                    Communication.router(finalData, str, connection);
                    
                    client.Close();
                });

                childSocketThread.Start();
            }
        }

    }
}
