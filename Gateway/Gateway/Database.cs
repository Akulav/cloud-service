using System;
using System.Data.SQLite;
using System.IO;

namespace Gateway
{
    class Database
    {
        public static readonly string[] dbNames = { "logins.mdf", "datas.mdf", "caches.mdf", "replication.mdf", "cachebackup.mdf" };
        public static void CreateDB()
        {

            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\databases"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\databases");

                for (int i = 0; i < dbNames.Length-2; i++)
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

                var db1 = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + dbNames[4]);
                var connect1 = $@"URI=file:{db1}";
                File.WriteAllText(db1, null);
                var conn1 = new SQLiteConnection(connect1);
                conn1.Open();
                var command1 = new SQLiteCommand(conn1)
                {
                    CommandText = @"CREATE TABLE data(id INTEGER PRIMARY KEY, cache VARCHAR(250))"
                };
                command1.ExecuteNonQuery();
            }
        }

        public static SQLiteConnection GetDB(string service)
        {
            CreateDB();
            SQLiteConnection connection = null;
            if (service == "user")
            {
                var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + dbNames[0]);
                string connectionString = $@"URI=file:{dbName}"; connection = new SQLiteConnection(connectionString);
                connection.Open();
            }
            else if (service == "data")
            {
                var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + dbNames[1]);
                string connectionString = $@"URI=file:{dbName}"; connection = new SQLiteConnection(connectionString);
                connection.Open();
            }
            else if (service == "cache")
            {
                var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + dbNames[2]);
                string connectionString = $@"URI=file:{dbName}"; connection = new SQLiteConnection(connectionString);
                connection.Open();
            }

            else if (service == "replication")
            {
                var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + dbNames[3]);
                string connectionString = $@"URI=file:{dbName}"; connection = new SQLiteConnection(connectionString);
                connection.Open();
            }

            else if (service == "cachebackup")
            {
                Console.WriteLine("CALLING LOCAL CACHE BACKUP");
                var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + dbNames[4]);
                string connectionString = $@"URI=file:{dbName}"; connection = new SQLiteConnection(connectionString);
                connection.Open();
            }
            return connection;
        }

        public static void ProcessRequest(string service, string data_string, bool client)
        {
            SQLiteConnection connection = GetDB(service);
            string query = @"SELECT * FROM data";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            var i = 1;
        read:
            var Table = cmd.ExecuteReader();

            while (true)
            {
                try
                {
                    for (int j = 0; j < i; j++)
                    {
                        Table.Read();
                    }
                    i++;

                    if (client)
                    {
                        Console.WriteLine("HERE1");
                        Console.WriteLine("sent to client.");
                        Console.WriteLine(data_string);
                        Communication.Send_response(data_string, "localhost", "13");
                        if (DataProcessor.wordArray(data_string.ToCharArray())[0] == "1")
                        {
                            Console.WriteLine("HERE2");
                            Communication.Send_response(data_string, Table[2].ToString(), Table[0].ToString());
                        }
                        break;
                    }

                    Communication.Send_response(data_string, Table[2].ToString(), Table[0].ToString());
                    connection.Dispose();
                    break;
                }
                catch
                {
                    try
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

                    catch { break; }
                }
            }
        }

        public static void InsertData(SQLiteConnection connection, string id, string ip)
        {
            try
            {
                var cmd = new SQLiteCommand(connection)
                {
                    CommandText = $"INSERT INTO data(id, errors, ip) VALUES('{id}', '0', '{ip}')"
                };
                cmd.ExecuteNonQuery();
            }
            catch { }
        }

        public static void InsertDataCache(SQLiteConnection connection, string data)
        {           
                var cmd = new SQLiteCommand(connection)
                {
                    CommandText = $"INSERT INTO data(cache) VALUES('{data}')"
                };
                cmd.ExecuteNonQuery();          
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

    }
}

