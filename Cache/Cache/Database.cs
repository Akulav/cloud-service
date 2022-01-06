using System;
using System.Data.SQLite;
using System.IO;

namespace Cache
{
    class Database
    {
        public static SQLiteConnection connectDB()
        {
            var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + "cache.mdf");

            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\databases"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\databases");


                var con = $@"URI=file:{dbName}";
                File.WriteAllText(dbName, null);
                var connect = new SQLiteConnection(con);
                connect.Open();
                var cmd = new SQLiteCommand(connect)
                {
                    CommandText = @"CREATE TABLE data(hash VARCHAR(20), reply VARCHAR(20), id INTEGER PRIMARY KEY)"
                };
                cmd.ExecuteNonQuery();

            }
            var conect = $@"URI=file:{dbName}";
            var connection = new SQLiteConnection(conect);
            connection.Open();


            return connection;
        }

        public static void insertCacheReply(SQLiteConnection connection, string reply, string hash)
        {
            Console.WriteLine(reply);
            Console.WriteLine(hash);

            try
            {
                Console.WriteLine(reply);
                Console.WriteLine(hash);
                var processed_reply = DataProcessor.wordArray(reply.ToCharArray());
                string insertreplyquery = $"INSERT INTO data (reply,hash, id) VALUES ('{processed_reply[0]}','{hash}', '{processed_reply[1]}')";
                SQLiteCommand cmd = new SQLiteCommand(insertreplyquery, connection);
                cmd.ExecuteNonQuery();
            }

            catch { }
        }


        public static string checkHash(SQLiteConnection connection, string hash)
        {
            string getreplyquery = $"SELECT reply FROM data WHERE hash = '{hash}'";
            string getidquery = $"SELECT id FROM data WHERE hash = '{hash}'";

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
