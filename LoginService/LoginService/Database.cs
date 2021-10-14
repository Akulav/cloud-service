using System;
using System.Data.SqlClient;
using System.Threading;

namespace LoginService
{
    class Database
    {
        public static void signup(string username, string password, SqlConnection  connection)
        {

            string id = DataProcessor.RandomString(10);

            string checkuserquery = $"SELECT COUNT(id)FROM[dbo].[Data] WHERE username = '{username}'";

            SqlCommand cmdcheck = new SqlCommand(checkuserquery, connection);
            string result = cmdcheck.ExecuteScalar().ToString();

            if (result == "1")
            {
                Communications.send_response("2", "localhost", 130);
            }

            else
            {
                string insertuserquery = $"INSERT INTO [dbo].[Data] (id, username, password) VALUES ('{id}','{username}','{password}')";
                SqlCommand cmd = new SqlCommand(insertuserquery, connection);
                cmd.ExecuteNonQuery();
                Communications.send_response("3","localhost", 130);
            }

        }

        public static SqlConnection connectDB()
        {
            SqlConnection connection = null;
            Thread thread = new Thread(() => {
                string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\akula\Documents\users.mdf; Integrated Security = True; Connect Timeout = 30";
                connection = new SqlConnection(connectionString);
                connection.Open();
                
            });
            thread.Start();
            thread.Join();
            return connection;
        }

        public static void login(string username, string password, string hash ,SqlConnection connection)
        {
            string checkloginquery = $"SELECT COUNT(id)FROM [dbo].[Data] WHERE username = '{username}' AND password = '{password}'";
            string getidquery = $"SELECT id FROM [dbo].[Data] WHERE username = '{username}' AND password = '{password}'";
            
            SqlCommand cmd = new SqlCommand(checkloginquery, connection);        
            string result = cmd.ExecuteScalar().ToString();
            if (result == "1")
            {
                SqlCommand idget = new SqlCommand(getidquery, connection);
                string id = idget.ExecuteScalar().ToString();
                Console.WriteLine(id);
                Communications.send_response("1" +" " + id + " " + hash, "localhost", 130);
            }

            else
            {
                Communications.send_response("0", "localhost", 130);
            }
        }

        public static void router(string[] data, SqlConnection connection)
        {
            if (data[0] == "signup")
            {
                signup(data[1], data[2], connection);
            }

            if (data[0] == "login" || data[0] == "loginNoCache")
            {
                login(data[1], data[2], data[3] ,connection);
            }
        }


    }
}
