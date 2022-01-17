using Community.CsharpSqlite.SQLiteClient;
using System.IO;

namespace DB1
{
    class Database
    {
        public static SqliteConnection connectDB()
        {
            var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + "db.mdf");

            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\databases"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\databases");


                var con = $@"URI=file:{dbName}";
                File.WriteAllText(dbName, null);
                var connect = new SqliteConnection(con);
                connect.Open();
                using (var cmd = connect.CreateCommand())
                {
                    cmd.CommandText = @"CREATE TABLE data(id INTEGER PRIMARY KEY, hash VARCRHAR(250))";
                    cmd.ExecuteNonQuery();
                };
                

            }
            var conect = $@"URI=file:{dbName}";
            var connection = new SqliteConnection(conect);
            connection.Open();


            return connection;
        }

        public static void write(SqliteConnection connection, string data)
        {
            string insertquery = $"INSERT INTO data (hash) VALUES ('{data}')";
            SqliteCommand cmd = new SqliteCommand(insertquery, connection);
            cmd.ExecuteNonQuery();
        }       

        public static void router(string[] data, SqliteConnection connection)
        {
            if (data[0] == "read")
            {
                string readquery = $"SELECT * FROM data";
                SqliteCommand cmd = new SqliteCommand(readquery, connection);
                var Table = cmd.ExecuteReader();
                while(Table.Read()){
                    try
                    {
                        Communications.send_response("sync" + " " +Table[1].ToString(), "localhost", int.Parse(data[2]));
                    }
                    catch { }
                }
               
            }

            else 
            {
                if (data[1].Length > 4) { write(connection, data[0] + " " + data[1] + " " + data[2]); }
               
            }
        }
    }
}
