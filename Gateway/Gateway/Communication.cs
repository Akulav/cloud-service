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

        public static void send_log(string data, string ip, string port)
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


            catch { Console.WriteLine("ERROR"); }

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

        public static void router(string[] data, string data_string)
        {

            //status(user, data, cache, connection);

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

            else if (data[0]=="signup")
            {
                SqlConnection connection = Database.getDB("user");
                string query = @"SELECT * FROM [dbo].[Table]";
                SqlCommand cmd = new SqlCommand(query, connection);
                read:
                var Table = cmd.ExecuteReader();

                while (Table.Read())
                {
                    try
                    {
                        //  send_response("health", Table[0].ToString(), Table[2].ToString(), "user", connection);
                        //Console.WriteLine(data_string + " " + Table[0].ToString() + Table[1].ToString());
                        send_response(data_string, Table[2].ToString(), Table[0].ToString(), "user", connection);
                        Table.Close();
                        break;
                    }
                    catch 
                    {
                        Console.WriteLine("ERROR");
                        string id = Table[0].ToString();
                        int fails = int.Parse(Table[1].ToString());
                        Table.Close();

                        if (fails < 3)
                        {
                            string insertquery = $"UPDATE [dbo].[Table] SET errors = ('{fails + 1}') WHERE id = ('{id}')";
                            SqlCommand cmd3 = new SqlCommand(insertquery, connection);
                            cmd3.ExecuteNonQuery();
                        }

                        else
                        {
                            string dropquery = $"DELETE FROM [dbo].[Table] WHERE id = '{id}'";
                            SqlCommand cmd2 = new SqlCommand(dropquery, connection);
                            cmd2.ExecuteNonQuery();
                        }
                        goto read;
                    }
                }         
            }
            /*
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
            */
        }

        public static void listen(int port)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            //int round_robin = 0;
            //string[] logger_ports = { "111", "112" };
            //int logger_length = logger_ports.Length;
 
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

                    //Console.WriteLine("WOW" + round_robin);

                    //if (round_robin >= logger_length) { round_robin = 0; send_log(finalData[1], "localhost", logger_ports[round_robin]); round_robin++; }

                    //else
                   // {
                    //    Console.WriteLine("HERE HERE HERE" + round_robin);
                        //send_log(finalData[1], "localhost", logger_ports[round_robin]);
                   //     round_robin++;
                    //}

                    router(finalData, str);

                    client.Close();
                });

                childSocketThread.Start();
            }
        }

    }
}

