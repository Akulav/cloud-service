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
            Random rnd = new Random();
            string id = DataProcessor.RandomString(50);
            string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\akula\Documents\users.mdf; Integrated Security = True; Connect Timeout = 30";

            string checkuserquery = $"SELECT COUNT(id)FROM[dbo].[Data] WHERE username = '{username}'";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand cmdcheck = new SqlCommand(checkuserquery, connection);
            string result = cmdcheck.ExecuteScalar().ToString();

            if (result == "1")
            {
                DataProcessor.send_response("Username Taken");
            }

            else
            {
                string query = $"INSERT INTO [dbo].[Data] (id, username, password) VALUES ('{id}','{username}','{password}')";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                DataProcessor.send_response("Succesfully signed up");
            }

        }

        public static void login(string username, string password)
        {
            string query = $"SELECT COUNT(id)FROM [dbo].[Data] WHERE username = '{username}' AND password = '{password}'";
            string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\akula\Documents\users.mdf; Integrated Security = True; Connect Timeout = 30";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);        
            string result = cmd.ExecuteScalar().ToString();
            if (result == "1")
            {
                DataProcessor.send_response("Logged in");
            }

            else
            {
                DataProcessor.send_response("Wrong data");
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
