using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LoginService
{
    class Database
    {
        public static void signup(string username, string password)
        {

            string id = DataProcessor.RandomString(10);
            string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\akula\Documents\users.mdf; Integrated Security = True; Connect Timeout = 30";

            string checkuserquery = $"SELECT COUNT(id)FROM[dbo].[Data] WHERE username = '{username}'";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand cmdcheck = new SqlCommand(checkuserquery, connection);
            string result = cmdcheck.ExecuteScalar().ToString();

            if (result == "1")
            {
                DataProcessor.send_response("2", "localhost", 130);
            }

            else
            {
                string insertuserquery = $"INSERT INTO [dbo].[Data] (id, username, password) VALUES ('{id}','{username}','{password}')";
                SqlCommand cmd = new SqlCommand(insertuserquery, connection);
                cmd.ExecuteNonQuery();
                DataProcessor.send_response("3","localhost", 130);
            }

        }

        public static void login(string username, string password)
        {
            string checkloginquery = $"SELECT COUNT(id)FROM [dbo].[Data] WHERE username = '{username}' AND password = '{password}'";
            string getidquery = $"SELECT id FROM [dbo].[Data] WHERE username = '{username}' AND password = '{password}'";
            string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\akula\Documents\users.mdf; Integrated Security = True; Connect Timeout = 30";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand(checkloginquery, connection);        
            string result = cmd.ExecuteScalar().ToString();
            if (result == "1")
            {
                SqlCommand idget = new SqlCommand(getidquery, connection);
                string id = idget.ExecuteScalar().ToString();
                Console.WriteLine(id);
                DataProcessor.send_response("1" +" " + id, "localhost", 130);
            }

            else
            {
                DataProcessor.send_response("0", "localhost", 130);
            }
        }

        public static void router(string[] data)
        {
            if (data[0] == "signup")
            {
                signup(data[1], data[2]);
            }

            if (data[0] == "login")
            {
                login(data[1], data[2]);
            }
        }


    }
}
