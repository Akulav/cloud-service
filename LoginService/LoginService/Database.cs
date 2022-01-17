using System;
using Community.CsharpSqlite.SQLiteClient;
using System.IO;

namespace LoginService
{
    class Database
    {
        public static void signup(string username, string password, SqliteConnection connection)
        {

            string checkuserquery = $"SELECT COUNT(id) FROM data WHERE username = '{username}'";

            SqliteCommand cmdcheck = new SqliteCommand(checkuserquery, connection);
            string result = cmdcheck.ExecuteScalar().ToString();

            if (result == "1")
            {
                Communications.send_response("2", "localhost", 130);
            }

            else
            {
                string insertuserquery = $"INSERT INTO data (username, password) VALUES ('{username}','{password}')";
                SqliteCommand cmd = new SqliteCommand(insertuserquery, connection);
                cmd.ExecuteNonQuery();
                Communications.send_response("3","localhost", 130);
            }

        }

        public static SqliteConnection connectDB()
        {
            var dbName = (Directory.GetCurrentDirectory() + "\\logindatabases" + "\\" + "login.mdf");

            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\logindatabases"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\logindatabases");


                var con = $@"URI=file:{dbName}";
                File.WriteAllText(dbName, null);
                var connect = new SqliteConnection(con);
                connect.Open();
                using (var cmd = connect.CreateCommand())
                {
                    cmd.CommandText = @"CREATE TABLE data(id INTEGER PRIMARY KEY, username VARCRHAR(250), password VARCRHAR(250))";
                    cmd.ExecuteNonQuery();
                };
                

            }
            var conect = $@"URI=file:{dbName}";
            var connection = new SqliteConnection(conect);
            connection.Open();


            return connection;
        }

        public static void login(string username, string password, string hash , SqliteConnection connection)
        {
            string checkloginquery = $"SELECT COUNT(id)FROM data WHERE username = '{username}' AND password = '{password}'";
            string getidquery = $"SELECT id FROM data WHERE username = '{username}' AND password = '{password}'";

            SqliteCommand cmd = new SqliteCommand(checkloginquery, connection);        
            string result = cmd.ExecuteScalar().ToString();
            if (result == "1")
            {
                SqliteCommand idget = new SqliteCommand(getidquery, connection);
                string id = idget.ExecuteScalar().ToString();
                Console.WriteLine(id);
                Communications.send_response("1" +" " + id + " " + hash, "localhost", 130);
            }

            else
            {
                Communications.send_response("0", "localhost", 130);
            }
        }

        public static void router(string[] data, SqliteConnection connection)
        {

            if (data[0] == "signup")
            {
                signup(data[1], data[2], connection);
            }

            else if (data[0] == "login" || data[0] == "loginNoCache")
            {
                login(data[1], data[2], data[3], connection);
            }

            else { Console.WriteLine(data[0]); }
            
        }
    }
}
