using System;
using System.Data.SQLite;
using System.IO;

namespace Cache
{
    class Database
    {
        public static readonly string[] dbNames = { "cache1.mdf", "cache2.mdf", "cache3.mdf", "replication.mdf" };

        public static void createDB()
        {

            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\databases"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\databases");

                for (int i = 0; i < dbNames.Length - 1; i++)
                {
                    var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + dbNames[i]);
                    var con = $@"URI=file:{dbName}";
                    File.WriteAllText(dbName, null);
                    var connection = new SQLiteConnection(con);
                    connection.Open();
                    var cmd = new SQLiteCommand(connection)
                    {
                        CommandText = @"CREATE TABLE data(hash VARCHAR(20), reply VARCHAR(20), id INTEGER)"
                    };
                    cmd.ExecuteNonQuery();
                }

                var db = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + dbNames[3]);
                var connect = $@"URI=file:{db}";
                File.WriteAllText(db, null);
                var conn = new SQLiteConnection(connect);
                conn.Open();
                var command = new SQLiteCommand(conn)
                {
                    CommandText = @"CREATE TABLE data(id INTEGER PRIMARY KEY, data VARCHAR(250), ip VARCRHAR(250), port VARCRHAR(250))"
                };
                command.ExecuteNonQuery();


                try
                {

                    Communication.send_response("read localhost 70", "localhost", 15001);

                }

                catch
                {
                    try
                    {
                        Communication.send_response("read localhost 70", "localhost", 15002);
                    }

                    catch
                    {
                        try
                        {
                            Communication.send_response("read localhost 70", "localhost", 15003);
                        }

                        catch { Console.WriteLine("ALL BACKUPS OFFLINE"); }
                    }
                }
            }
        }

        public static void insertCacheReply(string reply, string hash, bool broad)
        {
            Console.WriteLine(reply);
            Console.WriteLine(hash);

            if (reply.Length > 4)
            {
                var processed_reply = DataProcessor.wordArray(reply.ToCharArray());
                if (broad)
                {
                    Communication.Broadcast(processed_reply[0] + " " + hash + " " + processed_reply[1]);
                }



                for (int i = 0; i < dbNames.Length - 1; i++)
                {
                    var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + dbNames[i]);
                    string insertreplyquery = $"INSERT INTO data (reply,hash, id) VALUES ('{processed_reply[0]}','{hash}', '{processed_reply[1]}')";
                    string connectionString = $@"URI=file:{dbName}"; SQLiteConnection connection = new SQLiteConnection(connectionString);
                    connection.Open();
                    SQLiteCommand cmd = new SQLiteCommand(insertreplyquery, connection);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void insertCacheReplySync(string reply, string hash, bool broad)
        {
            Console.WriteLine(reply);
            Console.WriteLine(hash);

            if (reply.Length > 4)
            {
                var processed_reply = DataProcessor.wordArray(reply.ToCharArray());


                for (int i = 0; i < dbNames.Length - 1; i++)
                {
                    var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + dbNames[i]);
                    string insertreplyquery = $"INSERT INTO data (reply,hash, id) VALUES ('{processed_reply[0]}','{hash}', '{processed_reply[2]}')";
                    string connectionString = $@"URI=file:{dbName}"; SQLiteConnection connection = new SQLiteConnection(connectionString);
                    connection.Open();
                    SQLiteCommand cmd = new SQLiteCommand(insertreplyquery, connection);
                    cmd.ExecuteNonQuery();
                }
            }
        }



        public static void InsertError(SQLiteConnection connection, string data, string ip, string port)
        {
            try
            {
                var cmd = new SQLiteCommand(connection)
                {
                    CommandText = $"INSERT INTO data(data, ip, port) VALUES('{data}', '{ip}', '{port}')"
                };
                cmd.ExecuteNonQuery();
            }
            catch { }
        }


        public static string checkHash(string hash)
        {
            createDB();
            string getreplyquery = $"SELECT reply FROM data WHERE hash = '{hash}'";
            string getidquery = $"SELECT id FROM data WHERE hash = '{hash}'";

            Random rnd = new Random();
            int dbIndex = rnd.Next(2);
            var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + dbNames[dbIndex]);
            string connectionString = $@"URI=file:{dbName}"; SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();

            SQLiteCommand cmd = new SQLiteCommand(getreplyquery, connection);
            var table = cmd.ExecuteReader();
            table.Read();
            try
            {
                if (table[0].Equals("1"))
                {
                    SQLiteCommand replyget = new SQLiteCommand(getreplyquery, connection);
                    SQLiteCommand idget = new SQLiteCommand(getidquery, connection);
                    string reply = replyget.ExecuteScalar().ToString();
                    string id = idget.ExecuteScalar().ToString();
                    Console.WriteLine(reply + " " + id);
                    return reply + " " + id;
                }

                else
                {
                    return null;
                }
            }

            catch { return null; }
        }
    }
}
