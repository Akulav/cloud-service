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
        public static void send_response(string data, string ip, string port, string service, SqlConnection connection)
        {

            try
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

            
            catch { Console.WriteLine("ERROR"); Database.insertServiceERROR(connection, service); }

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

        public static void status(string[] user, string[] data, string[] cache, SqlConnection connection)
        {
            send_response("health", user[0], user[1], "user", connection);
            send_response("health", data[0], data[1], "data", connection);
            send_response("health", cache[0], cache[1], "cache", connection);
        }

        public static string[] getUser(SqlConnection connection)
        {
            string[] user = null;
            Thread thread = new Thread(() =>
            {
                user = Database.getAddress(connection, "user");
            });
            thread.Start();
            thread.Join();
            return user;
        }

        public static string[] getCache(SqlConnection connection)
        {
            string[] cache = null;
            Thread thread = new Thread(() =>
            {
                cache = Database.getAddress(connection, "cache");
            });
            thread.Start();
            thread.Join();
            return cache;
        }


        public static string[] getData(SqlConnection connection)
        {
            string[] data = null;
            Thread thread = new Thread(() =>
            {
                data = Database.getAddress(connection, "data");
            });
            thread.Start();
            thread.Join();
            return data;
        }

        public static void router(string[] data, string data_string, SqlConnection connection, string[] user, string[] dataA, string[] cache)
        {

            //status(user, data, cache, connection);

            if (data[0] == "whitelist")
            {
                Database.insertServicePort(connection, data[2], data[1]);
                Database.insertServiceIP(connection, data[3], data[1]);
            }

            else if (data[0] == "healthRply")
            {
                Console.WriteLine("Service: " + data[1] + " is alive.");
            }

            else if (data[0]=="signup")
            {
                send_response("health", user[0], user[1], "user", connection);
                send_response(data_string,user[0],user[1], "user", connection);          
            }

            else if (data[0] == "login")
            {
                send_response("health", cache[0], cache[1], "cache", connection);
                send_response(data_string, cache[0], cache[1], "cache", connection);
            }

            else if (data[0] == "connect" || data[0] == "upload" || data[0] == "download")
            {
                send_response("health", dataA[0], dataA[1], "data", connection);
                send_response(data_string, dataA[0], dataA[1], "data", connection);
            }

            else if (data[0] == "loginNoCache")
            {
                send_response(data_string, user[0], user[1], "user", connection);
            }

            else
            {
                Console.WriteLine(data_string + " WOW");
                send_to_client(data_string);
                send_response("health", cache[0], cache[1], "cache", connection);
                send_response(data_string , cache[0], cache[1], "cache", connection);
            }
        }

        public static void listen(int port, SqlConnection connection, string[] user, string[] dataA, string[] cache)
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
                    Communication.router(finalData, str, connection, user , dataA, cache);

                    client.Close();
                });

                childSocketThread.Start();
            }
        }

    }
}

