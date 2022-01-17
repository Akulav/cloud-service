using System;
using Community.CsharpSqlite.SQLiteClient;

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



        public static void router(string[] data, string data_string)
        {

            if (data[0] == "login")
            {

                string reply = Database.checkHash(data[3]);
                if (reply == null)
                {                
                    data[0] = "loginNoCache";
                    string converted = String.Join(" ", data);
                    send_response(converted, "localhost", 130);
                    Console.WriteLine("converted: " + converted);
                }

                else
                {
                    Console.WriteLine("HERE");
                    send_response(reply, "localhost", 130);
                }
            }

            else if (data[0] == "sync")
            {
                Console.WriteLine("MMM: "+ data[1] + " " + data[2] + " " + data[3]);
                Database.insertCacheReplySync(data[1] + " " + data[2] + " " + data[3], data[2],false);
            }

            else
            {
                Console.WriteLine("TO BE CACHED:" + data_string);
                Database.insertCacheReply(data_string, data[data.Length - 1],true);
            }

        }

        public static void Send_response(string data, string ip, string port)
        {
            TcpClient tcpClient = new TcpClient(ip, int.Parse(port));
            using NetworkStream ns = tcpClient.GetStream();
            using BufferedStream bs = new BufferedStream(ns);
            byte[] messageBytesToSend = Encoding.UTF8.GetBytes(data);
            bs.Write(messageBytesToSend, 0, messageBytesToSend.Length);
        }



        public static void Broadcast(string data)
        {
            Console.WriteLine("BACKUP CACHE...");

            var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + Database.dbNames[3]);
            string connectionString = $@"URI=file:{dbName}"; SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            try { Send_response(data, "localhost", "15001"); } catch { Database.InsertError(connection, data, "localhost", "15001"); }
            try { Send_response(data, "localhost", "15002"); } catch { Database.InsertError(connection, data, "localhost", "15002"); }
            try { Send_response(data, "localhost", "15003"); } catch { Database.InsertError(connection, data, "localhost", "15003"); }


            using var read = new SqliteCommand(@"SELECT * FROM data", connection);
         

            var Table = read.ExecuteReader();

            while (Table.Read())
            {
                broadAsync(Table[0].ToString(), Table[1].ToString(), Table[2].ToString(), Table[3].ToString(), connection);
            }
        }

        public static void broadAsync(string id, string data, string ip, string port, SqliteConnection con)
        {
            Thread thread = new Thread(delegate ()
            {
                try
                {
                    Send_response(data, ip, port);
                    using (var delete =con.CreateCommand())
                    {
                        delete.CommandText = $"DELETE FROM data WHERE id = '{id}'";
                        delete.ExecuteNonQuery();
                    };
                    
                }
                catch { }
            });
            thread.Start();
        }


        public static void listen(int port)
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

                    router(finalData, str);
                    
                    client.Close();
                });

                childSocketThread.Start();
            }
        }

    }
}
