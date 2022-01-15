using System;
using System.Data.SQLite;
using System.IO;

namespace Gateway
{
    class Database
    {
        public static readonly string[] dbNames = { "logins.mdf", "loggers.mdf", "datas.mdf", "caches.mdf", "dbs.mdf", "replication.mdf" };
        public static void createDB()
        {

            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\databases"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\databases");

                for (int i = 0; i < 5; i++)
                {
                    var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + dbNames[i]);
                    var con = $@"URI=file:{dbName}";
                    File.WriteAllText(dbName, null);
                    var connection = new SQLiteConnection(con);
                    connection.Open();
                    var cmd = new SQLiteCommand(connection)
                    {
                        CommandText = @"CREATE TABLE data(id INTEGER PRIMARY KEY, errors VARCRHAR(250), ip VARCRHAR(250))"
                    };
                    cmd.ExecuteNonQuery();
                }

                var db = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + dbNames[5]);
                var connect = $@"URI=file:{db}";
                File.WriteAllText(db, null);
                var conn = new SQLiteConnection(connect);
                conn.Open();
                var command = new SQLiteCommand(conn)
                {
                    CommandText = @"CREATE TABLE data(id INTEGER PRIMARY KEY, data VARCHAR(250), ip VARCRHAR(250), port VARCRHAR(250))"
                };
                command.ExecuteNonQuery();
            }
        }

        public static SQLiteConnection getDB(string service)
        {
            createDB();
            SQLiteConnection connection = null;
            if (service == "user")
            {
                var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + dbNames[0]);
                string connectionString = $@"URI=file:{dbName}"; connection = new SQLiteConnection(connectionString);
                connection.Open();
            }
            else if (service == "data")
            {
                var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + dbNames[2]);
                string connectionString = $@"URI=file:{dbName}"; connection = new SQLiteConnection(connectionString);
                connection.Open();
            }
            else if (service == "cache")
            {
                var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + dbNames[3]);
                string connectionString = $@"URI=file:{dbName}"; connection = new SQLiteConnection(connectionString);
                connection.Open();
            }

            else if (service == "db")
            {
                var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + dbNames[4]);
                string connectionString = $@"URI=file:{dbName}"; connection = new SQLiteConnection(connectionString);
                connection.Open();
            }

            else if (service == "replication")
            {
                var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + dbNames[5]);
                string connectionString = $@"URI=file:{dbName}"; connection = new SQLiteConnection(connectionString);
                connection.Open();
            }
            return connection;
        }

        public static void processRequest(string service, string data_string, bool client)
        {
            SQLiteConnection connection = getDB(service);
            string query = @"SELECT * FROM data";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            var i = 1;
        read:
            var Table = cmd.ExecuteReader();

            while (true)
            {
                try
                {
                    if (client)
                    {
                        Console.WriteLine("sent to client.");
                        Communication.send_to_client(data_string);
                    }

                    for (int j = 0; j < i; j++)
                    {
                        Table.Read();
                    }
                    i++;

                    Communication.send_response(data_string, Table[2].ToString(), Table[0].ToString());
                    connection.Dispose();
                    break;
                }
                catch
                {
                    Console.WriteLine("ERROR");
                    Console.WriteLine(i);
                    string id = Table[0].ToString();
                    int fails = int.Parse(Table[1].ToString());
                    Table.Close();


                    if (fails < 3)
                    {
                        string insertquery = $"UPDATE data SET errors = ('{fails + 1}') WHERE id = ('{id}')";
                        SQLiteCommand cmd3 = new SQLiteCommand(insertquery, connection);
                        cmd3.ExecuteNonQuery();
                    }

                    else
                    {
                        string dropquery = $"DELETE FROM data WHERE id = '{id}'";
                        SQLiteCommand cmd2 = new SQLiteCommand(dropquery, connection);
                        cmd2.ExecuteNonQuery();
                        i--;
                    }
                    goto read;
                }
            }
        }

        public static void insertData(SQLiteConnection connection, string id, string ip)
        {
            try
            {
                string insertquery = $"INSERT INTO data(id, errors, ip) VALUES('{id}', '0', '{ip}')";
                var cmd = new SQLiteCommand(connection)
                {
                    CommandText = $"INSERT INTO data(id, errors, ip) VALUES('{id}', '0', '{ip}')"
                };
                cmd.ExecuteNonQuery();
            }
            catch { }
        }

        public static void insertError(SQLiteConnection connection, string data, string ip, string port)
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

    }
}

