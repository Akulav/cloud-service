using System;
using System.Data.SQLite;
using System.IO;

namespace Cache
{
    class Database
    {
        public static readonly string[] dbNames = { "cache1.mdf", "cache2.mdf", "cache3.mdf"};

        public static void createDB()
        {

            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\databases"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\databases");

                for (int i = 0; i < 2; i++)
                {
                    var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + dbNames[i]);
                    var con = $@"URI=file:{dbName}";
                    File.WriteAllText(dbName, null);
                    var connection = new SQLiteConnection(con);
                    connection.Open();
                    var cmd = new SQLiteCommand(connection)
                    {
                        CommandText = @"CREATE TABLE data(hash VARCHAR(20), reply VARCHAR(20), id INTEGER PRIMARY KEY)"
                    };
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void insertCacheReply(string reply, string hash)
        {
            Console.WriteLine(reply);
            Console.WriteLine(hash);
            createDB();
            for (int i = 0; i< dbNames.Length; i++)
            {
                var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + dbNames[i]);
                var processed_reply = DataProcessor.wordArray(reply.ToCharArray());
                string insertreplyquery = $"INSERT INTO data (reply,hash, id) VALUES ('{processed_reply[0]}','{hash}', '{processed_reply[1]}')";
                string connectionString = $@"URI=file:{dbName}";SQLiteConnection connection = new SQLiteConnection(connectionString);
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(insertreplyquery, connection);
                cmd.ExecuteNonQuery();
            }
        }


        public static string checkHash(string hash)
        {
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
