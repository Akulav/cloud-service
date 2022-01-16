using System;
using System.Data.SQLite;
using System.IO;

namespace LoginService
{
    class Database
    {
        public static void signup(string username, string password, SQLiteConnection connection)
        {

            string checkuserquery = $"SELECT COUNT(id) FROM data WHERE username = '{username}'";

            SQLiteCommand cmdcheck = new SQLiteCommand(checkuserquery, connection);
            string result = cmdcheck.ExecuteScalar().ToString();

            if (result == "1")
            {
                Communications.send_response("2", "localhost", 130);
            }

            else
            {
                string insertuserquery = $"INSERT INTO data (username, password) VALUES ('{username}','{password}')";
                SQLiteCommand cmd = new SQLiteCommand(insertuserquery, connection);
                cmd.ExecuteNonQuery();
                Communications.send_response("3","localhost", 130);
            }

        }

        public static SQLiteConnection connectDB()
        {
            var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + "login.mdf");

            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\databases"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\databases");


                var con = $@"URI=file:{dbName}";
                File.WriteAllText(dbName, null);
                var connect = new SQLiteConnection(con);
                connect.Open();
                var cmd = new SQLiteCommand(connect)
                {
                    CommandText = @"CREATE TABLE data(id INTEGER PRIMARY KEY, username VARCRHAR(250), password VARCRHAR(250))"
                };
                cmd.ExecuteNonQuery();

            }
            var conect = $@"URI=file:{dbName}";
            var connection = new SQLiteConnection(conect);
            connection.Open();


            return connection;
        }

        public static void login(string username, string password, string hash , SQLiteConnection connection)
        {
            string checkloginquery = $"SELECT COUNT(id)FROM data WHERE username = '{username}' AND password = '{password}'";
            string getidquery = $"SELECT id FROM data WHERE username = '{username}' AND password = '{password}'";

            SQLiteCommand cmd = new SQLiteCommand(checkloginquery, connection);        
            string result = cmd.ExecuteScalar().ToString();
            if (result == "1")
            {
                SQLiteCommand idget = new SQLiteCommand(getidquery, connection);
                string id = idget.ExecuteScalar().ToString();
                Console.WriteLine(id);
                Communications.send_response("1" +" " + id + " " + hash, "localhost", 130);
            }

            else
            {
                Communications.send_response("0", "localhost", 130);
            }
        }

        public static void router(string[] data, SQLiteConnection connection)
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
