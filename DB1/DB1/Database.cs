using System.Data.SQLite;
using System.IO;

namespace DB1
{
    class Database
    {
        public static SQLiteConnection connectDB()
        {
            var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + "db.mdf");

            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\databases"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\databases");


                var con = $@"URI=file:{dbName}";
                File.WriteAllText(dbName, null);
                var connect = new SQLiteConnection(con);
                connect.Open();
                var cmd = new SQLiteCommand(connect)
                {
                    CommandText = @"CREATE TABLE data(id INTEGER PRIMARY KEY, hash VARCRHAR(250))"
                };
                cmd.ExecuteNonQuery();

            }
            var conect = $@"URI=file:{dbName}";
            var connection = new SQLiteConnection(conect);
            connection.Open();


            return connection;
        }

        public static void write(SQLiteConnection connection, string data)
        {
            string insertquery = $"INSERT INTO data (hash) VALUES ('{data}')";
            SQLiteCommand cmd = new SQLiteCommand(insertquery, connection);
            cmd.ExecuteNonQuery();
        }       

        public static void router(string[] data, SQLiteConnection connection)
        {



            if (data[0] == "read")
            {
                string readquery = $"SELECT * FROM data";
                SQLiteCommand cmd = new SQLiteCommand(readquery, connection);
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
