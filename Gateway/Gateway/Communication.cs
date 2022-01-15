using System;
using System.Data.SQLite;
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

            else if (data[0] == "healthRply")
            {
                Console.WriteLine("Service: " + data[1] + " is alive.");
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
            }

        }

        public static void Broadcast(string data)
        {
            try { Send_response(data, "localhost", "15001"); } catch { Database.InsertError(Database.GetDB("replication"), data, "localhost", "15001"); }
            try { Send_response(data, "localhost", "15002"); } catch { Database.InsertError(Database.GetDB("replication"), data, "localhost", "15002"); }
            try { Send_response(data, "localhost", "15003"); } catch { Database.InsertError(Database.GetDB("replication"), data, "localhost", "15003"); }

            SQLiteConnection con = Database.GetDB("replication");

            var read = new SQLiteCommand(con)
            {
                CommandText = @"SELECT * FROM data"
            };

            var Table = read.ExecuteReader();

            while (Table.Read())
            {
                broadAsync(Table[0].ToString(), Table[1].ToString(), Table[2].ToString(), Table[3].ToString(), con);
            }
        }

        public static void broadAsync(string id, string data, string ip, string port, SQLiteConnection con)
        {
                Thread thread = new Thread(delegate ()
                {
                    try
                    {
                        Send_response(data, ip, port);
                        var delete = new SQLiteCommand(con)
                        {
                            CommandText = $"DELETE FROM data WHERE id = '{id}'"
                        };
                        delete.ExecuteNonQuery();
                    }
                    catch { }
                });
            thread.Start();
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
                    Broadcast("write Meow");
                    Router(finalData, str);

                    client.Close();
                });

                childSocketThread.Start();
            }
        }

    }
}